using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.GetMyDetails
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
                _logger.LogInformation("Get My Details failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(Permissions.UserRead, query.UserId))
            {
                _logger.LogInformation("Get My Details failed for user: {UserId}. User does not have permission", query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }
            
            _logger.LogInformation("Get My Details succeeded for user: {UserId}", query.UserId);
            return Result.Success(new Response()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            });
        }
    }
}