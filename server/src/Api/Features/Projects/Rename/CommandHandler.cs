using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Application.Extensions;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Features.Projects.Rename
{
    public sealed class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<ProjectCacheOptions> _projectCacheOptions;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<CommandHandler> _logger;
        
        public CommandHandler(
            AppDbContext dbContext,
            ICacheProvider cacheProvider,
            IOptions<ProjectCacheOptions> projectCacheOptions,
            IValidator<Command> validator,
            ILogger<CommandHandler> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _projectCacheOptions = projectCacheOptions;
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
            
            if (command.ProjectId == Guid.Empty)
            {
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. Project not found", command.UserId, command.ProjectId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "RENAMEPROJECT_PROJECT_NOT_FOUND",
                    "We couldn't locate this project. Please try again later or contact support if the problem persists."));
            }

            var projectCacheKey = ProjectCacheKeys.GetById(command.ProjectId);
            var project = await _cacheProvider.ReadThroughAsync(projectCacheKey, _projectCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Projects
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == command.ProjectId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Project: {ProjectId} retrieval failed", command.ProjectId);
                    return null;
                }
            });

            if (project == null)
            {
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. Project not found", command.UserId, command.ProjectId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.NotFound,
                    "RENAMEPROJECT_PROJECT_NOT_FOUND",
                    "We couldn't locate this project. Please try again later or contact support if the problem persists."));
            }
            
            var projectsCacheKey = ProjectCacheKeys.GetMyProjects(command.UserId);
            var projects = await _cacheProvider.ReadThroughAsync(projectsCacheKey, _projectCacheOptions.Value, async () =>
            {
                try
                {
                    return await _dbContext.Projects
                        .AsNoTracking()
                        .Where(p => p.CreatedById == command.UserId)
                        .ToListAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Projects by user: {UserId} retrieval failed", command.UserId);
                    return null;
                }
            }) ?? [];

            var hasSameProjectName = projects
                .Where(p => p.Id != project.Id)
                .Any(p => string.Equals(p.Name, command.Name));

            if (hasSameProjectName)
            {
                _logger.LogInformation("Rename Project failed for user: {UserId} in project: {ProjectId}. Duplicate projects: {ProjectName} found", command.UserId, command.ProjectId, command.Name);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.Conflict,
                    "RENAMEPROJECT_DUPLICATE_PROJECT",
                    "Unable to rename project, an existing project with the same name already exists."));
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
                _ = _dbContext.Projects.Attach(project);

                project.Name = command.Name;

                _ = _dbContext.Projects.Update(project);
                _ = await _dbContext.SaveChangesAsync(cancellationToken);
                
                _cacheProvider.Remove([
                    ProjectCacheKeys.GetById(project.Id),
                    ProjectCacheKeys.GetMyProjects(command.UserId),
                    ProjectCacheKeys.GetProjectsImInvolvedWith(command.UserId)
                ]);

                _logger.LogInformation("Rename Project succeeded for user: {UserId} in project: {ProjectId}", command.UserId, command.ProjectId);
                return Result.Success(new Response()
                {
                    ProjectId = project.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rename Project failed for user: {UserId} in project: {ProjectId}. Unexpected error occurred", command.UserId, command.ProjectId);
                return Result.Failure<Response>(new Error(
                    ErrorStatus.Failure, 
                    "RENAMEPROJECT_UNEXPECTED_ERROR", 
                    "An unexpected error occurred when creating your new project. Please try again later or contact support if the problem persists."));
            }
        }
    }
}