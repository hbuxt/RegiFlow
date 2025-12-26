using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.InviteUser
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext, 
            IProjectService projectService,
            IUserService userService,
            IRoleService roleService,
            IPermissionService permissionService, 
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _userService = userService;
            _roleService = roleService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAsync(command.Roles, RoleScope.Project);
            
            if (roles.Count != command.Roles.Count)
            {
                _logger.LogInformation("Project: {ProjectId} invitation failed for user: {UserId}. One of the roles did not exist", command.ProjectId, command.UserId);
                return Result.Failure<Response>(Errors.RoleNotFound());
            }
            
            var project = await _projectService.GetAsync(command.ProjectId);
            
            if (project == null)
            {
                _logger.LogInformation("Project: {Projectid} invitation failed for user: {UserId}. Project does not exist", command.ProjectId, command.UserId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectInvitationsInvite, command.UserId, command.ProjectId))
            {
                _logger.LogInformation("Project: {ProjectId} invitation failed for user: {UserId}. User does not have permission", command.ProjectId, command.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }
            
            var recipient = await _userService.GetAsync(command.Email);

            if (recipient == null)
            {
                _logger.LogInformation("Project: {ProjectId} invitation failed for user: {UserId}. User: {UserEmail} does not exist", command.ProjectId, command.UserId, command.Email);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            var isInProject = await _dbContext.ProjectUsers
                .AsNoTracking()
                .AnyAsync(pu => pu.UserId == recipient.Id, cancellationToken);

            if (isInProject)
            {
                _logger.LogInformation("Project: {ProjectId} invitation for user: {UserId}. Duplicate user in project.", command.ProjectId, command.UserId);
                return Result.Failure<Response>(Errors.DuplicateUser());
            }

            var pendingInvitationExists = await _dbContext.Invitations
                .AsNoTracking()
                .AnyAsync(i => i.RegardingId == project.Id &&
                               i.Status == NotificationStatus.Pending &&
                               i.RecipientId == recipient.Id, cancellationToken);

            if (pendingInvitationExists)
            {
                _logger.LogInformation("Project: {ProjectId} invitation for user: {UserId}. Duplicate invitation in project.", command.ProjectId, command.UserId);
                return Result.Failure<Response>(Errors.DuplicateInvitation());
            }

            var invitation = new Invitation()
            {
                Id = Guid.NewGuid(),
                Type = NotificationType.Invitation,
                Status = NotificationStatus.Pending,
                RecipientId = recipient.Id,
                RegardingId = command.ProjectId,
                SentById = command.UserId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(3),
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Content = $"You've been invited to {project.Name} as {string.Join(", ", roles.Select(r => r.Name))}",
                Data = new InvitationData()
                {
                    Roles = roles.Select(r => r.Id).ToList()
                }
            };

            _ = _dbContext.Notifications.Add(invitation);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
                
            _logger.LogInformation("Project: {ProjectId} invitation to recipient: {RecipientId} succeded for user: {UserId}", command.ProjectId, recipient.Id, command.UserId);
            return Result.Success(new Response()
            {
                Id = invitation.Id
            });
        }
    }
}