using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.Users.Me.Permissions
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            AppDbContext dbContext,
            IProjectService projectService, 
            IPermissionService permissionService, 
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (!await _projectService.ExistsAsync(query.ProjectId))
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId} in project: {ProjectId}. Project not found", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectPermissionsRead, query.UserId, query.ProjectId))
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId} in project: {ProjectId}. User does not have permission", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            try
            {
                var permissions = await _dbContext.ProjectUsers
                    .AsNoTracking()
                    .Where(pu => pu.ProjectId == query.ProjectId && pu.UserId == query.UserId)
                    .SelectMany(pu => pu.ProjectUserRoles)
                    .SelectMany(pur => pur.Role!.RolePermissions)
                    .Select(rp => rp.Permission!)
                    .ToListAsync(cancellationToken);
                
                _logger.LogInformation("List My Permissions failed for user: {UserId} in project: {ProjectId}", query.UserId, query.ProjectId);
                return Result.Success(new Response()
                {
                    Permissions = permissions.Select(p => p.Name).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "List My Permissions failed for user: {UserId} in project: {ProjectId}. An unexpected error occurred", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}