using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Identity;
using Api.Infrastructure.Localization;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Features.Auth.Register
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IErrorLocalizer _localizer;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly IOptions<RoleCacheOptions> _roleCacheOptions;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IErrorLocalizer localizer,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IOptions<UserCacheOptions> userCacheOptions,
            IOptions<RoleCacheOptions> roleCacheOptions,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _localizer = localizer;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _userCacheOptions = userCacheOptions;
            _roleCacheOptions = roleCacheOptions;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Registration failed for user: {Email}. Validation errors occurred: {@Errors}", command.Email, validationResult.ToFormattedDictionary());
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

            if (existingUser != null)
            {
                _logger.LogInformation("Registration failed for user: {Email}. User already exists", command.Email);
                return Result.Failure<Response>(new Error(ErrorStatus.Conflict, ErrorCodes.RegisterEmailAlreadyExists, _localizer.GetMessage(ErrorCodes.RegisterEmailAlreadyExists)));
            }

            var roleCacheKey = RoleCacheKeys.GetByName(RoleNames.StandardUser);
            var role = await _cacheProvider.ReadThroughAsync(roleCacheKey, _roleCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Roles
                        .AsNoTracking()
                        .Where(r => r.Scope == RoleScope.Application)
                        .FirstOrDefaultAsync(r => r.Name == RoleNames.StandardUser, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Role: {RoleName} retrieval failed", RoleNames.StandardUser);
                    return null;
                }
            });

            if (role == null)
            {
                _logger.LogError("Registration failed for user: {Email}. Role not found", command.Email);
                return Result.Failure<Response>(new Error(ErrorStatus.NotFound, ErrorCodes.GeneralUnexpectedError, _localizer.GetMessage(ErrorCodes.GeneralUnexpectedError)));
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var user = new User()
                    {
                        Id = Guid.NewGuid(),
                        Email = command.Email,
                        HashedPassword = _passwordHasher.HashPassword(command.Password),
                        CreatedAt = DateTime.UtcNow
                    };
                    
                    _ = _dbContext.Users.Add(user);
                    
                    var userRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    _ = _dbContext.UserRoles.Add(userRole);
                    _ = await _dbContext.SaveChangesAsync(cancellationToken);
                    
                    await transaction.CommitAsync(cancellationToken);
                    
                    _cacheProvider.Remove(UserCacheKeys.GetByEmail(command.Email));
                    
                    _logger.LogInformation("Registration succeeded for user: {UserId}", user.Id);
                    return Result.Success(new Response()
                    {
                        AccessToken = _tokenGenerator.GenerateAccessToken(user)
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogError(ex, "Registration failed for user: {Email}. Unexpected error occurred", command.Email);
                    return Result.Failure<Response>(new Error(ErrorStatus.Failure, ErrorCodes.GeneralUnexpectedError, _localizer.GetMessage(ErrorCodes.GeneralUnexpectedError)));
                }
            }
        }
    }
}