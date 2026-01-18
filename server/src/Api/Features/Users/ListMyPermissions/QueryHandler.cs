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

namespace Api.Features.Users.ListMyPermissions
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
                _logger.LogInformation("List My Permissions failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserPermissionsRead, query.UserId))
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId}. User does not have permission", query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            try
            {
                var permissions = await _dbContext.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.UserId == query.UserId)
                    .SelectMany(ur => ur.Role!.RolePermissions)
                    .Select(rp => rp.Permission!)
                    .ToListAsync(cancellationToken);
                
                _logger.LogInformation("List My Permissions succeeded for user: {UserId}", query.UserId);
                return Result.Success(new Response()
                {
                    Permissions = permissions
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.Name)
                        .ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "List My Permissions failed for user: {UserId}. Unexpected error occurred.", query.UserId);
                return Result.Failure<Response>(Errors.SomethingWentWrong());
            }
        }
    }
}