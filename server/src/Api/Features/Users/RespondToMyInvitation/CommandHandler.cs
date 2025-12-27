using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.RespondToMyInvitation
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext, 
            IPermissionService permissionService,
            IValidator<Command> validator, 
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _permissionService = permissionService;
            _validator = validator;
            _logger = logger;
        }
        
        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.ToFormattedDictionary();
                
                _logger.LogInformation("Respond to invitation failed for user: {UserId} for invitation: " +
                    "{invitationId}. Validation errors occurred: {@Errors}", command.UserId, command.InvitationId, 
                    validationErrors);
                return Result.Failure<Response>(validationErrors);
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserInvitationsUpdate, command.UserId))
            {
                _logger.LogWarning("Respond to invitation failed for user: {UserId} for invitation: {InvitationId}. " +
                    "User does not have permission", command.UserId, command.InvitationId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var invitation = await _dbContext.Invitations
                .SingleOrDefaultAsync(i => 
                    i.RecipientId == command.UserId &&
                    i.Status == NotificationStatus.Pending && 
                    i.Id == command.InvitationId);

            if (invitation == null)
            {
                _logger.LogInformation("Respond to invitation failed for user: {UserId} for invitation: " +
                    "{InvitationId}. Invitation not found", command.UserId, command.InvitationId);
                return Result.Failure<Response>(Errors.InvitationNotFound());
            }

            if (invitation.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogInformation("Respond to invitation failed for user: {UserId} for invitation: " +
                    "{Invitation}. Invitation has expired", command.UserId, command.InvitationId);
                return Result.Failure<Response>(Errors.InvitationHasExpired());
            }

            var status = Enum.Parse<InvitationStatus>(command.Status, ignoreCase: true);

            if (status == InvitationStatus.Accept)
            {
                invitation.Status = NotificationStatus.Resolved;

                var projectUser = new ProjectUser()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = invitation.RegardingId,
                    UserId = command.UserId,
                    JoinedAt = DateTime.UtcNow
                };

                _ = _dbContext.ProjectUsers.Add(projectUser);

                if (invitation.Data?.Roles?.Count > 0)
                {
                    var projectUserRoles = invitation.Data.Roles.Select(r => new ProjectUserRole()
                    {
                        Id = Guid.NewGuid(),
                        ProjectUserId = projectUser.Id,
                        RoleId = r,
                        AssignedAt = DateTime.UtcNow
                    });

                    _dbContext.ProjectUserRoles.AddRange(projectUserRoles);
                }
            }
            else
            {
                invitation.Status = NotificationStatus.Declined;
            }
            
            _ = await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Respond to invitation succeeded for user: {UserId} for invitation: " +
                "{InvitationId}", command.UserId, command.InvitationId);
            return Result.Success(new Response()
            {
                Id = invitation.Id
            });
        }
    }
}