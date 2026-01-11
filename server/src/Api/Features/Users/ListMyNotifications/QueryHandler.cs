using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Behaviours;
using Api.Domain.Constants;
using Api.Domain.Entities;
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

            if (!await _permissionService.IsAuthorizedAsync(PermissionNames.UserNotificationsRead, query.UserId))
            {
                _logger.LogInformation("List my notifications for user: {UserId} failed. User does not have permission", 
                    query.UserId);
                return Result.Failure<Response>(Errors.UserNotAuthorized());
            }

            var invitationsQuery = _dbContext.Notifications
                .OfType<Invitation>()
                .AsNoTracking()
                .Where(n => n.RecipientId == query.UserId)
                .Include(i => i.SentBy)
                .Include(i => i.Regarding);
            
            var notifications = await invitationsQuery
                .Select(i => new
                {
                    i.Id,
                    i.Type,
                    i.Status,
                    i.CreatedAt,
                    i.Token,
                    i.ExpiresAt,
                    SentBy = i.SentBy != null ? new UserDto { Id = i.SentBy.Id, Email = i.SentBy.Email } : null,
                    Regarding = i.Regarding != null ? new ProjectDto { Id = i.Regarding.Id, Name = i.Regarding.Name, Description = i.Regarding.Description } : null,
                    RoleIds = i.Data != null ? i.Data.Roles : new List<Guid>()
                })
                .ToListAsync();
            
            var allRoleIds = notifications.SelectMany(n => n.RoleIds!).Distinct().ToList();

            var rolesMap = await _dbContext.Roles
                .AsNoTracking()
                .Where(r => allRoleIds.Contains(r.Id))
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToDictionaryAsync(r => r.Id);
            
            _logger.LogInformation("List my notifications for user: {UserId} succeeded", query.UserId);
            return Result.Success(new Response()
            {
                Notifications = notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type.ToString(),
                    Status = n.Status.ToString(),
                    CreatedAt = n.CreatedAt,
                    InvitationDetails = n.RoleIds == null ? null : new InvitationDetailsDto
                    {
                        Token = n.Token,
                        ExpiresAt = n.ExpiresAt,
                        SentBy = n.SentBy,
                        Regarding = n.Regarding,
                        Roles = n.RoleIds.Select(id => rolesMap[id]).ToList()
                    }
                }).OrderByDescending(n => n.CreatedAt).ToList()
            });
        }
    }
}