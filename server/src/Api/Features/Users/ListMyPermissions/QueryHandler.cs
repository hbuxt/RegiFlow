using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.ListMyPermissions
{
    public sealed class QueryHandler : IQueryHandler<Query, Response>
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<QueryHandler> _logger;

        public QueryHandler(
            IUserService userService, 
            IPermissionService permissionService, 
            ILogger<QueryHandler> logger)
        {
            _userService = userService;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await _userService.GetAsync(query.UserId);

            if (user == null)
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(Permissions.UserPermissionsRead, user.Id))
            {
                _logger.LogInformation("List My Permissions failed for user: {UserId}. User does not have permission", user.Id);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var permissions = await _permissionService.ListForUserAsync(user.Id);
            
            _logger.LogInformation("List My Permissions succeeded for user: {UserId}", user.Id);
            return Result.Success(new Response()
            {
                Permissions = permissions.Select(p => p.Name).ToList()
            });
        }
    }
}