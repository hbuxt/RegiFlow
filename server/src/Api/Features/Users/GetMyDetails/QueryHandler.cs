using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.GetMyDetails
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
            var user = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => !u.IsDeleted && u.Id == query.UserId);

            if (user == null)
            {
                _logger.LogInformation("Get my details failed for user: {UserId}. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserRead, query.UserId))
            {
                _logger.LogInformation("Get my details failed for user: {UserId}. User does not have permission", 
                    query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }
            
            _logger.LogInformation("Get my details succeeded for user: {UserId}", query.UserId);
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