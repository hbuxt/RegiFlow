using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Constants;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Api.Features.Projects.Rename
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            IProjectService projectService,
            IPermissionService permissionService,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
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
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. Validation errors occurred: {@Errors}", command.UserId, command.ProjectId, validationResult.ToFormattedDictionary());
                return Result.Failure<Response>(validationResult.ToFormattedDictionary());
            }

            var project = await _projectService.GetAsync(command.ProjectId);

            if (project == null)
            {
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. Project not found", command.UserId, command.ProjectId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.RenameProject, command.UserId, project.Id))
            {
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. User does not have permission", command.UserId, project.Id);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var projects = await _projectService.ListByCreatorAsync(command.UserId);
            var hasSameProjectName = projects
                .Where(p => p.Id != project.Id)
                .Any(p => string.Equals(p.Name, command.Name));

            if (hasSameProjectName)
            {
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. Duplicate projects: {ProjectName} found", command.UserId, command.ProjectId, command.Name);
                return Result.Failure<Response>(Errors.DuplicateProjectName());
            }

            if (string.Equals(project.Name, command.Name))
            {
                _logger.LogInformation("Rename Project succeeded with no action taken for user: {UserId} in project: {ProjectId}", command.UserId, command.ProjectId);
                return Result.Success(new Response()
                {
                    ProjectId = project.Id
                });
            }

            try
            {
                _ = await _projectService.RenameAsync(project, command.UserId, command.Name);

                _logger.LogInformation("Rename Project succeeded for user: {UserId} in project: {ProjectId}", command.UserId, command.ProjectId);
                return Result.Success(new Response()
                {
                    ProjectId = project.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rename Project failed for user: {UserId} in project: {ProjectId}. Unexpected error occurred", command.UserId, command.ProjectId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}