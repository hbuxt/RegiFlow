using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.DeleteMyDetails
{
    public sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            IUserService userService,
            IPermissionService permissionService,
            ILogger<CommandHandler> logger)
        {
            _userService = userService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await _userService.GetAsync(command.UserId);
            
            if (user == null)
            {
                _logger.LogInformation("Delete My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(Permissions.UserDelete, command.UserId))
            {
                _logger.LogInformation("Delete My Details failed for user: {UserId}. User does not have permission", user.Id);
                return Result.Failure(Errors.UserNotAuthorized());
            }

            try
            {
                _ = await _userService.SoftDeleteAsync(user);
                
                _logger.LogInformation("Delete My Details succeeded for user: {UserId}", user.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete My Details failed for user: {UserId}. Unexpected error occurred", command.UserId);
                return Result.Failure(Errors.SomethingWentWrong());
            }
        }
    }
}