using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Identity;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Features.Auth.Login
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IOptions<UserCacheOptions> userCacheOptions,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _userCacheOptions = userCacheOptions;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Login failed for user: {Email}. Validation errors occurred: {@Errors}", command.Email, validationResult.ToFormattedDictionary());
                return Result.Failure<Response>(validationResult.ToFormattedDictionary());
            }
            
            var userCacheKey = UserCacheKeys.GetByEmail(command.Email);
            var existingUser = await _cacheProvider.ReadThroughAsync(userCacheKey, _userCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == command.Email, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User: {Email} retrieval failed", command.Email);
                    return null;
                }
            });

            if (existingUser == null)
            {
                _logger.LogInformation("Login failed for user: {Email}. User not found", command.Email);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound, 
                    "LOGIN_INVALID_CREDENTIALS", 
                    "The email or password you entered does not match an existing account."));
            }

            try
            {
                var isValidPassword = _passwordHasher.VerifyPassword(command.Password, existingUser.HashedPassword);

                if (!isValidPassword)
                {
                    _logger.LogInformation("Login failed for user: {Email}. Invalid password", command.Email);
                    return Result.Failure<Response>(new Error(
                        ErrorStatus.NotFound, 
                        "LOGIN_INVALID_CREDENTIALS", 
                        "The email or password you entered does not match an existing account."));
                }
                
                var accessToken = _tokenGenerator.GenerateAccessToken(existingUser);

                _logger.LogInformation("Login succeeded for user: {UserId}", existingUser.Id);
                return Result.Success(new Response()
                {
                    AccessToken = accessToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user: {Email}. Unexpected error occurred", command.Email);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.Failure,
                    "LOGIN_UNEXPECTED_ERROR",
                    "An unexpected error occurred when signing you in. Please try again later or contact support if the problem persists."));
            }
        }
    }
}