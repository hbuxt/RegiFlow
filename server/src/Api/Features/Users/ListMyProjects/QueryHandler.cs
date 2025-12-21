using System;
using System.Linq;
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

namespace Api.Features.Users.ListMyProjects
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly IOptions<ProjectCacheOptions> _projectCacheOptions;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<UserCacheOptions> userCacheOptions,
            IOptions<ProjectCacheOptions> projectCacheOptions,
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _userCacheOptions = userCacheOptions;
            _projectCacheOptions = projectCacheOptions;
            _logger = logger;
        }
        
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.UserId == null || query.UserId == Guid.Empty)
            {
                _logger.LogInformation("List My Roles failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "LISTMYPROJECTS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }
            
            var userCacheKey = UserCacheKeys.GetById(query.UserId.Value);
            var user = await _cacheProvider.ReadThroughAsync(userCacheKey, _userCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == query.UserId.Value, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User: {UserId} retrieval failed", query.UserId);
                    return null;
                }
            });
            
            if (user == null)
            {
                _logger.LogInformation("List My Roles failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "LISTMYPROJECTS_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }

            var projectsCacheKey = ProjectCacheKeys.GetProjectsImInvolvedWith(user.Id);
            var projects = await _cacheProvider.ReadThroughAsync(projectsCacheKey, _projectCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Projects
                        .AsNoTracking()
                        .Include(p => p.CreatedBy)
                        .Include(p => p.ProjectUsers)
                        .ThenInclude(pu => pu.User)
                        .Where(p => p.ProjectUsers.Any(pu => pu.UserId == user.Id))
                        .ToListAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User: {UserId} projects retrieval failed", user.Id);
                    return null;
                }
            }) ?? [];
            
            _logger.LogInformation("List My Projects succeeded for user: {UserId}", query.UserId);
            return Result.Success(new Response()
            {
                Projects = projects
                    .Select(p => new ProjectDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy == null ? null : new UserDto()
                        {
                            Id = p.CreatedBy.Id,
                            Email = p.CreatedBy.Email
                        }
                    })
                    .ToList()
            });
        }
    }
}