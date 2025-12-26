using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.ListMyRoles
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
                _logger.LogInformation("List My Roles failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserRolesRead, query.UserId))
            {
                _logger.LogInformation("List My Roles failed for user: {UserId}. User does not have permission", query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            try
            {
                var roles = await _dbContext.UserRoles
                    .AsNoTracking()
                    .Include(ur => ur.Role)
                    .Where(ur => ur.UserId == query.UserId)
                    .ToListAsync(cancellationToken);
                
                _logger.LogInformation("List My Roles succeeded for user: {UserId}", query.UserId);
                return Result.Success(new Response()
                {
                    Roles = roles
                        .Where(ur => ur.Role != null)
                        .Select(ur => new RoleDto()
                        {
                            Id = ur.Role!.Id,
                            Name = ur.Role.Name
                        })
                        .ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "List My Roles failed for user: {UserId}. An unexpected error occurred.", query.UserId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}