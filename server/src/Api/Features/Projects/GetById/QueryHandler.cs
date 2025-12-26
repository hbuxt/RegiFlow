using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.GetById
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;
        
        public QueryHandler(
            AppDbContext dbContext,
            IPermissionService permissionService,
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == query.ProjectId);

            if (project == null)
            {
                _logger.LogInformation("Get project failed for user: {UserId} in project: {ProjectId}. " +
                    "Project not found", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectRead, query.UserId, project.Id))
            {
                _logger.LogInformation("Get project failed for user: {UserId} in project: {ProjectId}. " +
                    "User does not have permission", query.UserId, project.Id);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            _logger.LogInformation("Get project succeeded for user: {UserId} in project: {ProjectId}", 
                query.UserId, query.ProjectId);
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