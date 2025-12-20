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

namespace Api.Features.Users.GetMyDetails
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<UserCacheOptions> userCacheOptions,
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _userCacheOptions = userCacheOptions;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.UserId == null || query.UserId == Guid.Empty)
            {
                _logger.LogInformation("Get My Details failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "GETMYDETAILS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }
            
            var userCacheKey = UserCacheKeys.GetById(query.UserId.Value);
            var user = await _cacheProvider.ReadThroughAsync(userCacheKey, _userCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == query.UserId.Value, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User: {UserId} retrieval failed", query.UserId);
                    return null;
                }
            });

            if (user == null)
            {
                _logger.LogInformation("Get My Details failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "GETMYDETAILS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }
            
            _logger.LogInformation("Get My Details succeeded for user: {UserId}", query.UserId);
            return Result.Success(new Response()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            });
        }
    }
}