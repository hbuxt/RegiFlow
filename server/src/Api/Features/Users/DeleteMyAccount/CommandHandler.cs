using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.DeleteMyAccount
{
    public sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext,
            IUserService userService,
            IPermissionService permissionService,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _userService = userService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            if (!await _userService.ExistsAsync(command.UserId))
            {
                _logger.LogInformation("Delete my account failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserDelete, command.UserId))
            {
                _logger.LogInformation("Delete my account failed for user: {UserId}. User does not have permission", 
                    command.UserId);
                return Result.Failure(Errors.UserNotAuthorized());
            }

            // TODO:
            // Think about what projects should be deleted.
            // Delete all projects where I am only project user?
            // Delete all projetcs where I am the owner?
            // Notifications?
            var user = await _dbContext.Users.FirstAsync(u => !u.IsDeleted && u.Id == command.UserId);
                
            user.FirstName = null;
            user.LastName = null;
            user.Email = Guid.NewGuid().ToString();
            user.HashedPassword = Guid.NewGuid().ToString();
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            _ = await _dbContext.SaveChangesAsync();
                
            _logger.LogInformation("Delete my account succeeded for user: {UserId}", command.UserId);
            return Result.Success();
        }
    }
}