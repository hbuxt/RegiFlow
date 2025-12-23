using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.ListMyProjects
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            IUserService userService,
            IProjectService projectService,
            IPermissionService permissionService,
            ILogger<QueryHandler> logger)
        {
            _userService = userService;
            _projectService = projectService;
            _permissionService = permissionService;
            _logger = logger;
        }
        
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await _userService.GetAsync(query.UserId);
            
            if (user == null)
            {
                _logger.LogInformation("List My Projects failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(Permissions.ProjectRead, query.UserId))
            {
                _logger.LogInformation("List My Projects failed for user: {UserId}. User does not have permission", query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var projects = await _projectService.ListByUserAsync(query.UserId);
            
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