using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Features.Users.ListMyNotifications
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
                _logger.LogInformation("List my notifications for user: {UserId} failed. User not found", query.UserId);
                return Result.Failure<Response>(Errors.UserNotFound());
            }

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.NotificationsRead, query.UserId))
            {
                _logger.LogInformation("List my notifications for user: {UserId} failed. User does not have permission", 
                    query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var notifications = await _dbContext.Notifications
                .AsNoTracking()
                .Where(n => n.RecipientId == query.UserId)
                .Select(n => new NotificationDto()
                {
                    Id = n.Id,
                    Type = n.Type.ToString(),
                    Status = n.Status.ToString(),
                    Content = n.Content,
                    CreatedAt = n.CreatedAt,
                    InvitationDetails = n.Type == NotificationType.Invitation
                        ? new InvitationDetailsDto
                        {
                            Token = ((Invitation)n).Token,
                            ExpiresAt = ((Invitation)n).ExpiresAt,
                            Regarding = new ProjectDto
                            {
                                Name = ((Invitation)n).Regarding!.Name
                            }
                        }
                        : null
                })
                .ToListAsync();
                
            _logger.LogInformation("List my notifications for user: {UserId} succeeded", query.UserId);
            return Result.Success(new Response()
            {
                Notifications = notifications
            });
        }
    }
}