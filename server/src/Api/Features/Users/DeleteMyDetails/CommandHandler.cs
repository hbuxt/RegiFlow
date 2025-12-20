using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Features.Users.DeleteMyDetails
{
    public sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<UserCacheOptions> userCacheOptions,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _userCacheOptions = userCacheOptions;
            _logger = logger;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            if (command.UserId == null || command.UserId == Guid.Empty)
            {
                _logger.LogInformation("Delete My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure(new Error(
                    ErrorStatus.NotFound,
                    "DELETEMYDETAILS_USER_NOT_FOUND",
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
                _logger.LogInformation("Delete My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure(new Error(
                    ErrorStatus.NotFound,
                    "DELETEMYDETAILS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                var id = user.Id;
                var email = user.Email;
                
                try
                {
                    _ = _dbContext.Users.Attach(user);

                    user.FirstName = null;
                    user.LastName = null;
                    user.Email = Guid.NewGuid().ToString();
                    user.HashedPassword = Guid.NewGuid().ToString();
                    user.IsDeleted = true;
                    user.DeletedAt = DateTime.UtcNow;

                    _ = _dbContext.Users.Update(user);
                    _ = await _dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                    
                    _cacheProvider.Remove([
                        UserCacheKeys.GetById(id),
                        UserCacheKeys.GetRolesById(id),
                        UserCacheKeys.GetByEmail(email)
                    ]);
                    
                    _logger.LogInformation("Delete My Details succeeded for user: {UserId}", id);
                    return Result.Success();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogError(ex, "Delete My Details failed for user: {UserId}. Unexpected error occurred", command.UserId);
                    return Result.Failure(new Error(
                        ErrorStatus.Failure,
                        "DELETEMYDETAILS_UNEXPECTED_ERROR",
                        "An unexpected error occurred when deleting your account. Please try again later or contact support if the problem persists."));
                }
            }
        }
    }
}