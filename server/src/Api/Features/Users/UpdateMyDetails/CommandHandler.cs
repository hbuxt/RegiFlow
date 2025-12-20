using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Features.Users.UpdateMyDetails
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<UserCacheOptions> userCacheOptions,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _userCacheOptions = userCacheOptions;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (command.UserId == null || command.UserId == Guid.Empty)
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "UPDATEMYDETAILS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }
            
            var userCacheKey = UserCacheKeys.GetById(command.UserId.Value);
            var user = await _cacheProvider.ReadThroughAsync(userCacheKey, _userCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == command.UserId.Value, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User: {UserId} retrieval failed", command.UserId);
                    return null;
                }
            });

            if (user == null)
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "UPDATEMYDETAILS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }
            
            var normalizedNewFirstName = string.IsNullOrWhiteSpace(command.FirstName) ? null : command.FirstName;
            var normalizedExistingFirstName = string.IsNullOrWhiteSpace(user.FirstName) ? null : user.FirstName;
            var hasFirstNameChanged = !string.Equals(normalizedNewFirstName, normalizedExistingFirstName);
                
            var normalizedNewLastName = string.IsNullOrWhiteSpace(command.LastName) ? null : command.LastName;
            var normalizedExistingLastName = string.IsNullOrWhiteSpace(user.LastName) ? null : user.LastName;
            var hasLastNameChanged = !string.Equals(normalizedNewLastName, normalizedExistingLastName);

            if (!hasFirstNameChanged && !hasLastNameChanged)
            {
                _logger.LogInformation("Update My Details succeeded with no action taken for user: {UserId}", user.Id);
                return Result.Success(new Response()
                {
                    Id = user.Id
                });
            }
            
            try
            {
                _ = _dbContext.Users.Attach(user);

                if (hasFirstNameChanged)
                {
                    user.FirstName = normalizedNewFirstName;
                }

                if (hasLastNameChanged)
                {
                    user.LastName = normalizedNewLastName;
                }
                
                _ = _dbContext.Users.Update(user);
                _ = await _dbContext.SaveChangesAsync(cancellationToken);
                
                _cacheProvider.Remove([
                    UserCacheKeys.GetById(user.Id),
                    UserCacheKeys.GetByEmail(user.Email)
                ]);
                
                _logger.LogInformation("Update My Details succeeded for user: {UserId}", user.Id);
                return Result.Success(new Response()
                {
                    Id = user.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update My Details failed for user: {UserId}. Unexpected error occurred", user.Id);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.Failure,
                    "UPDATEMYDETAILS_UNEXPECTED_ERROR",
                    "An unexpected error occurred when updating your details. Please try again later or contact support if the problem persists."));
            }
        }
    }
}