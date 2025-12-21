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

namespace Api.Features.Projects.GetById
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<ProjectCacheOptions> _projectCacheOptions;
        private readonly ILogger<QueryHandler> _logger;
        
        public QueryHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<ProjectCacheOptions> projectCacheOptions,
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _projectCacheOptions = projectCacheOptions;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.ProjectId == null || query.ProjectId == Guid.Empty)
            {
                _logger.LogInformation("Get Project By ID failed for user: {UserId} in project: {ProjectId}. Project not found", query.UserId, query.ProjectId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "GETPROJECTBYID_PROJECT_NOT_FOUND",
                    "We couldn't locate this project. Please try again later or contact support if the problem persists."));
            }

            var projectCacheKey = ProjectCacheKeys.GetById(query.ProjectId.Value);
            var project = await _cacheProvider.ReadThroughAsync(projectCacheKey, _projectCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Projects
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == query.ProjectId.Value, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Project: {ProjectId} retrieval failed", query.ProjectId);
                    return null;
                }
            });

            if (project == null)
            {
                _logger.LogInformation("Get Project By ID failed for user: {UserId} in project: {ProjectId}. Project not found", query.UserId, query.ProjectId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "GETPROJECTBYID_PROJECT_NOT_FOUND",
                    "We couldn't locate this project. Please try again later or contact support if the problem persists."));
            }

            _logger.LogInformation("Get Project By ID succeeded for user: {UserId} in project: {ProjectId}", query.UserId, query.ProjectId);
            return Result.Success(new Response()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            });
        }
    }
}