using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.ListPermissionsByUser
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            IProjectService projectService, 
            IPermissionService permissionService, 
            ILogger<QueryHandler> logger)
        {
            _projectService = projectService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetAsync(query.ProjectId);

            if (project == null)
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId} in project: {ProjectId}. Project not found", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(Permissions.ProjectPermissionsRead, query.UserId, project.Id))
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId} in project: {ProjectId}. User does not have permission", query.UserId, project.Id);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var permissions = await _permissionService.ListForUserAsync(query.UserId, project.Id);
            
            _logger.LogInformation("List My Permissions failed for user: {UserId} in project: {ProjectId}", query.UserId, project.Id);
            return Result.Success(new Response()
            {
                Permissions = permissions.Select(p => p.Name).ToList()
            });
        }
    }
}