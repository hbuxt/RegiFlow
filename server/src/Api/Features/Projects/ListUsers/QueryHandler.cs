using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.ListUsers
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
                _logger.LogInformation("List project users failed for user: {UserId} in project: {ProjectId}. " +
                "   Project not found", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectUsersRead, query.UserId, 
                query.ProjectId))
            {
                _logger.LogInformation("List project users failed for user: {UserId} in project: {ProjectId}. " +
                "   User does not have permission", query.UserId, query.ProjectId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var users = await _dbContext.ProjectUsers
                .AsNoTracking()
                .Include(pu => pu.User)
                .Include(pu => pu.ProjectUserRoles)
                .ThenInclude(pur => pur.Role)
                .Where(pu => pu.ProjectId == query.ProjectId)
                .ToListAsync();
                
            _logger.LogInformation("List project users succeeded for user: {UserId} in project: {ProjectId}", 
                query.ProjectId, query.UserId);
            return Result.Success(new Response()
            {
                Users = users
                    .Select(pu => new UserDto()
                    {
                        Id = pu.User!.Id,
                        Email = pu.User.Email,
                        Roles = pu.ProjectUserRoles
                            .Select(pur => new RoleDto()
                            {
                                Id = pur.Role!.Id,
                                Name = pur.Role.Name
                            })
                            .ToList()
                    })
                    .ToList()
            });
        }
    }
}