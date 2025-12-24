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

namespace Api.Features.Projects.Users.Members
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
                _logger.LogInformation("Project: {ProjectId} query failed for user: {UserId}. Project not found", query.ProjectId, query.UserId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectUsersRead, query.UserId, query.ProjectId))
            {
                _logger.LogInformation("Project: {ProjectId} query failed for user: {UserId}. User does not have permission", query.ProjectId, query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            try
            {
                var users = await _dbContext.ProjectUsers
                    .AsNoTracking()
                    .Include(pu => pu.User)
                    .Include(pu => pu.ProjectUserRoles)
                    .ThenInclude(pur => pur.Role)
                    .Where(pu => pu.ProjectId == query.ProjectId)
                    .ToListAsync(cancellationToken);
                
                _logger.LogInformation("Project: {ProjectId} query succeeded for user: {UserId}", query.ProjectId, query.UserId);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Project: {ProjectId} query failed for user: {UserId}. Unexpected error occurred", query.ProjectId, query.UserId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}