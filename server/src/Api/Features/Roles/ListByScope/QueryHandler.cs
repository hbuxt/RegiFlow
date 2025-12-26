using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Roles.ListByScope
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly AppDbContext _dbContext;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            AppDbContext dbContext, 
            IPermissionService permissionService, 
            ILogger<QueryHandler> logger)
        {
            _dbContext = dbContext;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.RolesRead, query.UserId))
            {
                _logger.LogInformation("List roles by scope: {RoleScope} failed for user: {UserId}. " +
                    "User does not have permission", query.Scope, query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var roles = await _dbContext.Roles
                .AsNoTracking()
                .Where(r => r.Scope == query.Scope)
                .ToListAsync();
                
            _logger.LogInformation("List roles by scope: {RoleScope} succeeded for user: {UserId}", 
                query.Scope, query.UserId);
            return Result.Success(new Response()
            {
                Roles = roles
                    .Select(r => new RoleDto()
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .ToList()
            });
        }
    }
}