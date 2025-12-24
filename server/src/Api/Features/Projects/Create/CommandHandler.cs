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

namespace Api.Features.Projects.Create
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            IUserService userService,
            IRoleService roleService,
            IPermissionService permissionService,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _userService = userService;
            _roleService = roleService;
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

            if (!await _userService.ExistsAsync(command.UserId))
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(Permissions.ProjectCreate, command.UserId))
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. User does not have permission", command.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            try
            {
                var isDuplicate = await _dbContext.Projects
                    .AsNoTracking()
                    .Where(p => p.CreatedById == command.UserId)
                    .AnyAsync(p => string.Equals(p.Name, command.Name), cancellationToken);
                
                if (isDuplicate)
                {
                    _logger.LogInformation("Create Project failed for user: {UserId}. Duplicate project: {ProjectName} found", command.UserId, command.Name);
                    return Result.Failure<Response>(Errors.DuplicateProjectName());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Project failed for user: {UserId}. An unexpected error occurred", command.UserId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
            
            var role = await _roleService.GetAsync(Roles.Owner, RoleScope.Project);

            if (role == null)
            {
                _logger.LogError("Create Project failed for user: {UserId}. Role not found", command.UserId);
                return Result.Failure<Response>(Errors.RoleNotFound());
            }
            
            try
            {
                var project = new Project()
                {
                    Id = Guid.NewGuid(),
                    CreatedById = command.UserId,
                    Name = command.Name,
                    Description = command.Description,
                    CreatedAt = DateTime.UtcNow
                };
                
                _ = _dbContext.Projects.Add(project);
                
                var projectUser = new ProjectUser()
                {
                    ProjectId = project.Id,
                    UserId = command.UserId,
                    JoinedAt = DateTime.UtcNow
                };
                
                _ = _dbContext.ProjectUsers.Add(projectUser);
                
                var projectOwner = new ProjectUserRole()
                {
                    Id = Guid.NewGuid(),
                    ProjectUserId = projectUser.Id,
                    RoleId = role.Id,
                    AssignedAt = DateTime.UtcNow
                };
                
                _ = _dbContext.ProjectUserRoles.Add(projectOwner);
                
                _ = await _dbContext.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Create Project succeeded for user: {UserId} with project: {ProjectId}", command.UserId, project.Id);
                return Result.Success(new Response()
                {
                    Name = project.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Project failed for user: {UserId}. Unexpected error occurred", command.UserId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}