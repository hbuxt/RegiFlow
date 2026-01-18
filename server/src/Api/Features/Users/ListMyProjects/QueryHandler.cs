using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.ListMyProjects
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            AppDbContext dbContext,
            IUserService userService,
            IPermissionService permissionService,
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _userService = userService;
            _permissionService = permissionService;
            _logger = logger;
        }
        
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (!await _userService.ExistsAsync(query.UserId))
            {
                _logger.LogInformation("List my projects failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.ProjectRead, query.UserId))
            {
                _logger.LogInformation("List my projects failed for user: {UserId}. User does not have permission", 
                    query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var projects = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.CreatedBy)
                .Where(p => p.ProjectUsers.Any(pu => pu.UserId == query.UserId))
                .ToListAsync();
                
            _logger.LogInformation("List my projects succeeded for user: {UserId}", query.UserId);
            return Result.Success(new Response()
            {
                Projects = projects
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new ProjectDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy == null ? null : new UserDto()
                        {
                            Id = p.CreatedBy.Id,
                            Email = p.CreatedBy.Email
                        }
                    })
                    .ToList()
            });
        }
    }
}