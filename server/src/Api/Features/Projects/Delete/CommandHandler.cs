using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.Delete
{
    public sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly AppDbContext _dbContext;
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext, 
            IProjectService projectService, 
            IPermissionService permissionService, 
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            if (!await _projectService.ExistsAsync(command.ProjectId))
            {
                _logger.LogInformation("Delete project failed for user: {UserId} in project: {ProjectId}. " +
                    "Project not found", command.UserId, command.ProjectId);
                return Result.Failure(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectDelete, command.UserId, command.ProjectId))
            {
                _logger.LogInformation("Delete project failed for user: {UserId} in project: {ProjectId}. " +
                        "User does not have permission", command.UserId, command.ProjectId);
                return Result.Failure(Errors.UserNotAuthorized());
            }

            var project = await _dbContext.Projects.FirstAsync(p => p.Id == command.ProjectId);
                
            var projectUsers = await _dbContext.ProjectUsers
                .Where(pu => pu.ProjectId == project.Id)
                .ToListAsync();

            var projectUserIds = projectUsers.Select(pu => pu.Id);
            var projectUserRoles = await _dbContext.ProjectUserRoles
                .Where(pur => projectUserIds.Contains(pur.ProjectUserId))
                .ToListAsync();

            _dbContext.ProjectUserRoles.RemoveRange(projectUserRoles);
            _dbContext.ProjectUsers.RemoveRange(projectUsers);
            _dbContext.Projects.Remove(project);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
                
            _logger.LogInformation("Delete project succeeded for user: {UserId} in project: {ProjectId}", 
                command.UserId, command.ProjectId);
            return Result.Success();
        }
    }
}