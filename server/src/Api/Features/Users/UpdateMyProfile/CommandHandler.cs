using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.UpdateMyProfile
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext,
            IUserService userService,
            IPermissionService permissionService,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
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

            if (!await _userService.ExistsAsync(command.UserId))
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserUpdate, command.UserId))
            {
                _logger.LogInformation("Update My Details failed for user: {UserId}. User does not have permission", command.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }
            
            try
            {
                var user = await _dbContext.Users.FirstAsync(u => u.Id == command.UserId, cancellationToken);

                var normalizedNewFirstName = string.IsNullOrWhiteSpace(command.FirstName) ? null : command.FirstName;
                var normalizedNewLastName = string.IsNullOrWhiteSpace(command.LastName) ? null : command.LastName;
                
                user.FirstName = normalizedNewFirstName;
                user.LastName = normalizedNewLastName;

                _ = await _dbContext.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Update My Details succeeded for user: {UserId}", command.UserId);
                return Result.Success(new Response()
                {
                    Id = user.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update My Details failed for user: {UserId}. Unexpected error occurred", command.UserId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}