using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.UpdateMyDetails
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            IUserService userService,
            IPermissionService permissionService,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _userService = userService;
            _permissionService = permissionService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. Validation errors occurred: {@Errors}", command.UserId, validationResult.ToFormattedDictionary());
                return Result.Failure<Response>(validationResult.ToFormattedDictionary());
            }

            var user = await _userService.GetAsync(command.UserId);

            if (user == null)
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UpdateMyDetails, command.UserId))
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. User does not have permission", user.Id);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }
            
            var normalizedNewFirstName = string.IsNullOrWhiteSpace(command.FirstName) ? null : command.FirstName;
            var normalizedExistingFirstName = string.IsNullOrWhiteSpace(user.FirstName) ? null : user.FirstName;
            var hasFirstNameChanged = !string.Equals(normalizedNewFirstName, normalizedExistingFirstName);
                
            var normalizedNewLastName = string.IsNullOrWhiteSpace(command.LastName) ? null : command.LastName;
            var normalizedExistingLastName = string.IsNullOrWhiteSpace(user.LastName) ? null : user.LastName;
            var hasLastNameChanged = !string.Equals(normalizedNewLastName, normalizedExistingLastName);

            if (!hasFirstNameChanged && !hasLastNameChanged)
            {
                _logger.LogInformation("Update My Details succeeded with no action taken for user: {UserId}", user.Id);
                return Result.Success(new Response()
                {
                    Id = user.Id
                });
            }
            
            try
            {
                _ = await _userService.UpdateAsync(user, normalizedNewFirstName, normalizedNewLastName);
                
                _logger.LogInformation("Update My Details succeeded for user: {UserId}", user.Id);
                return Result.Success(new Response()
                {
                    Id = user.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update My Details failed for user: {UserId}. Unexpected error occurred", user.Id);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}