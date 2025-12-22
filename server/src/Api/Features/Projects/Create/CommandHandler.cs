using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.Create
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            IUserService userService,
            IRoleService roleService,
            IProjectService projectService,
            IPermissionService permissionService,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _projectService = projectService;
            _permissionService = permissionService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. Validation errors occurred: {@Errors}", command.UserId, validationResult.ToFormattedDictionary());
                return Result.Failure<Response>(validationResult.ToFormattedDictionary());
            }

            var user = await _userService.GetAsync(command.UserId);
            
            if (user == null)
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.CreateProjects, command.UserId))
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. User does not have permission", user.Id);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var projects = await _projectService.ListByCreatorAsync(user.Id);

            if (projects.Any(p => string.Equals(p.Name, command.Name)))
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. Duplicate project: {ProjectName} found", user.Id, command.Name);
                return Result.Failure<Response>(Errors.DuplicateProjectName());
            }

            var role = await _roleService.GetAsync(RoleNames.Owner, RoleScope.Project);

            if (role == null)
            {
                _logger.LogError("Create Project failed for user: {UserId}. Role not found", user.Id);
                return Result.Failure<Response>(Errors.RoleNotFound());
            }

            try
            {
                var project = await _projectService.CreateAsync(
                    user, 
                    command.Name, 
                    command.Description, 
                    role);
                
                _logger.LogInformation("Create Project succeeded for user: {UserId} with project: {ProjectId}", user.Id, project.Id);
                return Result.Success(new Response()
                {
                    Name = project.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Project failed for user: {UserId}. Unexpected error occurred", user.Id);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}