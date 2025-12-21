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
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Features.Projects.Create
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<UserCacheOptions> _userCacheOptions;
        private readonly IOptions<RoleCacheOptions> _roleCacheOptions;
        private readonly IOptions<ProjectCacheOptions> _projectCacheOptions;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<UserCacheOptions> userCacheOptions,
            IOptions<RoleCacheOptions> roleCacheOptions,
            IOptions<ProjectCacheOptions> projectCacheOptions,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _userCacheOptions = userCacheOptions;
            _roleCacheOptions = roleCacheOptions;
            _projectCacheOptions = projectCacheOptions;
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
            
            if (command.UserId == null || command.UserId == Guid.Empty)
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "CREATEPROJECT_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }
            
            var userCacheKey = UserCacheKeys.GetById(command.UserId.Value);
            var user = await _cacheProvider.ReadThroughAsync(userCacheKey, _userCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == command.UserId.Value, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User: {UserId} retrieval failed", command.UserId);
                    return null;
                }
            });
            
            if (user == null)
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. User not found", command.UserId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "CREATEPROJECT_USER_NOT_FOUND",
                    "We couldn't locate your account. Please try again later or contact support if the problem persists."));
            }

            var projectsCacheKey = ProjectCacheKeys.GetMyProjects(user.Id);
            var projects = await _cacheProvider.ReadThroughAsync(projectsCacheKey, _projectCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Projects
                        .AsNoTracking()
                        .Where(p => p.CreatedById == user.Id)
                        .ToListAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Projects by user: {UserId} retrieval failed", user.Id);
                    return null;
                }
            }) ?? [];

            if (projects.Any(p => string.Equals(p.Name, command.Name)))
            {
                _logger.LogInformation("Create Project failed for user: {UserId}. Duplicate projects: {ProjectName} found", user.Id, command.Name);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.Conflict,
                    "CREATEPROJECT_DUPLICATE_PROJECT",
                    "Unable to create project, an existing project with the same name already exists."));
            }

            var roleCacheKey = RoleCacheKeys.GetByName(RoleNames.Owner);
            var role = await _cacheProvider.ReadThroughAsync(roleCacheKey, _roleCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Roles
                        .AsNoTracking()
                        .Where(r => r.Scope == RoleScope.Project)
                        .FirstOrDefaultAsync(r => r.Name == RoleNames.Owner, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Role: {RoleName} retrieval failed", RoleNames.Owner);
                    return null;
                }
            });

            if (role == null)
            {
                _logger.LogError("Create Project failed for user: {UserId}. Role not found", user.Id);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound, 
                    "CREATEPROJECT_UNEXPECTED_ERROR", 
                    "An unexpected error occurred when creating your new project. Please try again later or contact support if the problem persists."));
            }

            try
            {
                var project = new Project()
                {
                    Id = Guid.NewGuid(),
                    CreatedById = user.Id,
                    Name = command.Name,
                    Description = command.Description,
                    CreatedAt = DateTime.UtcNow
                };

                _ = _dbContext.Projects.Add(project);

                var projectUser = new ProjectUser()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project.Id,
                    UserId = user.Id,
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
                
                _cacheProvider.Remove([
                    ProjectCacheKeys.GetMyProjects(user.Id),
                    ProjectCacheKeys.GetProjectsImInvolvedWith(user.Id)
                ]);
                
                _logger.LogInformation("Create Project succeeded for user: {UserId} with project: {ProjectId}", user.Id, project.Id);
                return Result.Success(new Response()
                {
                    Name = project.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Project failed for user: {UserId}. Unexpected error occurred", user.Id);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.Failure, 
                    "CREATEPROJECT_UNEXPECTED_ERROR", 
                    "An unexpected error occurred when creating your new project. Please try again later or contact support if the problem persists."));
            }
        }
    }
}