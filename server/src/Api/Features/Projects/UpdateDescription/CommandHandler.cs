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

namespace Api.Features.Projects.UpdateDescription
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IProjectService _projectService;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            AppDbContext dbContext, 
            IProjectService projectService, 
            IPermissionService permissionService,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
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
                var validationErrors = validationResult.ToFormattedDictionary();
                
                _logger.LogInformation("Update project failed for user: {UserId} in project: {ProjectId}. " +
                    "Validation errors occurred: {@Errors}", command.UserId, command.ProjectId, validationErrors);
                return Result.Failure<Response>(validationErrors);
            }

            if (!await _projectService.ExistsAsync(command.ProjectId))
            {
                _logger.LogInformation("Update project failed for user: {UserId} in project: {ProjectId}. " +
                    "Project not found", command.UserId, command.ProjectId);
                return Result.Failure<Response>(Errors.ProjectNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectDescriptionUpdate, command.UserId, 
                command.ProjectId))
            {
                _logger.LogInformation("Update project failed for user: {UserId} in project: {ProjectId}. " +
                    "User does not have permission", command.UserId, command.ProjectId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var project = await _dbContext.Projects.FirstAsync(p => p.Id == command.ProjectId);
            var normalizedDescription = string.IsNullOrWhiteSpace(command.Description) ? null : command.Description;
                
            project.Description = normalizedDescription;

            _ = await _dbContext.SaveChangesAsync();
                
            _logger.LogInformation("Update project succeeded for user: {UserId} in project: {ProjectId}", 
                command.UserId, command.ProjectId);
            return Result.Success(new Response()
            {
                ProjectId = command.ProjectId
            });
        }
    }
}