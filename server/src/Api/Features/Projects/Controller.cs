using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Features.Projects
{
    [ApiController]
    [Route(EndpointRoutes.Projects)]
    [Authorize]
    public sealed class Controller : BaseController
    {
        public Controller(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route(EndpointRoutes.CreateProject, Name = EndpointNames.CreateProject)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Create.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> Create([FromBody] Create.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Create.Command(
                User.GetUserId(), 
                request?.Name,
                request?.Description), cancellationToken);
            
            return result.Match(
                _ => Results.CreatedAtRoute(EndpointNames.CreateProject, value: result.Value),
                _ => result.ToProblemDetails());
        }

        [HttpGet]
        [Route(EndpointRoutes.GetProjectById, Name = EndpointNames.GetProjectById)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(GetById.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> GetById([FromRoute] Guid? id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetById.Query(User.GetUserId(), id), cancellationToken);

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpPut]
        [Route(EndpointRoutes.UpdateProject, Name = EndpointNames.UpdateProject)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(UpdateDescription.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> Update([FromRoute] Guid? id, [FromBody] UpdateDescription.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateDescription.Command(
                User.GetUserId(), 
                id,
                request?.Description), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }

        [HttpPut]
        [Route(EndpointRoutes.RenameProject, Name = EndpointNames.RenameProject)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Rename.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> Rename([FromRoute] Guid? id, [FromBody] Rename.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Rename.Command(
                User.GetUserId(), 
                id,
                request?.Name), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpDelete]
        [Route(EndpointRoutes.DeleteProject, Name = EndpointNames.DeleteProject)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> DeleteProject([FromRoute] Guid? id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Delete.Command(User.GetUserId(), id), cancellationToken);

            return result.Match(
                Results.NoContent,
                _ => result.ToProblemDetails());
        }
        
        [HttpPost]
        [Route(EndpointRoutes.InviteUserToProject, Name = EndpointNames.InviteUserToProject)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(InviteUser.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> InviteUser([FromRoute] Guid? id, [FromBody] InviteUser.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new InviteUser.Command(
                User.GetUserId(), 
                id,
                request?.Email,
                request?.Roles), cancellationToken);
            
            return result.Match(
                _ => Results.CreatedAtRoute(EndpointNames.InviteUserToProject, value: result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListUsersInProject, Name = EndpointNames.ListUsersInProject)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ListUsers.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> ListUsersInProject([FromRoute] Guid? id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListUsers.Query(User.GetUserId(), id), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListMyPermissionsInProject, Name = EndpointNames.ListMyPermissionsInProject)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ListMyPermissions.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Projects)]
        public async Task<IResult> ListMyPermissionsInProject([FromRoute] Guid? id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListMyPermissions.Query(User.GetUserId(), id), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
    }
}