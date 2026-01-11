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
using Api.Domain.ValueObjects;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.InviteUser
{
    public sealed class CommandHandler : ICommandHandler<Command>
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

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.ToFormattedDictionary();
                
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                    "Validation errors occurred: {@Errors}", command.UserId, command.ProjectId, validationErrors);
                return Result.Failure(validationErrors);
            }
            
            var roles = await _dbContext.Roles
                .AsNoTracking()
                .Where(r => r.Scope == RoleScope.Project && command.Roles.Contains(r.Id))
                .ToListAsync();
            
            if (roles.Count != command.Roles.Count)
            {
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                    "One of the roles did not exist", command.UserId, command.ProjectId);
                return Result.Failure(Errors.RoleNotFound());
            }

            var project = await _dbContext.Projects
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == command.ProjectId);
            
            if (project == null)
            {
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                    "Project does not exist", command.UserId, command.ProjectId);
                return Result.Failure(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectInvitationsInvite, command.UserId, 
                command.ProjectId))
            {
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                    "User does not have permission", command.UserId, command.ProjectId);
                return Result.Failure(Errors.UserNotAuthorized());
            }

            var recipient = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => !u.IsDeleted && u.Email == command.Email);

            if (recipient == null)
            {
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                    "User: {UserEmail} does not exist", command.UserId, command.ProjectId, command.Email);
                return Result.Success();
            }

            var isInProject = await _dbContext.ProjectUsers
                .AsNoTracking()
                .AnyAsync(pu => pu.UserId == recipient.Id);

            if (isInProject)
            {
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                "   Duplicate user in project.", command.UserId, command.ProjectId);
                return Result.Failure(Errors.DuplicateUser());
            }

            var pendingInvitationExists = await _dbContext.Invitations
                .AsNoTracking()
                .AnyAsync(i => i.RegardingId == project.Id &&
                               i.Status == NotificationStatus.Pending &&
                               i.ExpiresAt >= DateTime.UtcNow &&
                               i.RecipientId == recipient.Id);

            if (pendingInvitationExists)
            {
                _logger.LogInformation("Project invitation failed for user: {UserId} in project: {ProjectId}. " +
                    "Duplicate invitation in project.", command.UserId, command.ProjectId);
                return Result.Success();
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
            _ = await _dbContext.SaveChangesAsync();
                
            _logger.LogInformation("Project invitation succeded for user: {UserId} in " +
                "project: {ProjectId} to recipient: {RecipientId}", command.UserId, command.ProjectId, recipient.Id);
            return Result.Success();
        }
    }
}