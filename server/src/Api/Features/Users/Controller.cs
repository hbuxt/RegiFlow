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

namespace Api.Features.Users
{
    [ApiController]
    [Route(EndpointRoutes.Users)]
    [Authorize]
    public sealed class Controller : BaseController
    {
        public Controller(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        [Route(EndpointRoutes.GetMyDetails, Name = EndpointNames.GetMyDetails)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Me.Details.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> GetMyDetails(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Details.Query(User.GetUserId()), cancellationToken);

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpPut]
        [Route(EndpointRoutes.UpdateMyDetails, Name = EndpointNames.UpdateMyDetails)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Me.Update.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> UpdateMyDetails([FromBody] Me.Update.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Update.Command(
                User.GetUserId(),
                request?.FirstName,
                request?.LastName), cancellationToken);

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpDelete]
        [Route(EndpointRoutes.DeleteMyAccount, Name = EndpointNames.DeleteMyAccount)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> DeleteMyAccount(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Delete.Command(User.GetUserId()), cancellationToken);

            return result.Match(
                Results.NoContent,
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListMyRoles, Name = EndpointNames.ListMyRoles)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Me.Roles.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Roles.Query(User.GetUserId()), cancellationToken);

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }

        [HttpGet]
        [Route(EndpointRoutes.ListMyProjects, Name = EndpointNames.ListMyProjects)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Me.Projects.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyProjects(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Projects.Query(User.GetUserId()), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListMyPermissions, Name = EndpointNames.ListMyPermissions)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Me.Permissions.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyPermissions(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Permissions.Query(User.GetUserId()), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListMyNotifications, Name = EndpointNames.ListMyNotifications)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Me.Notifications.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyNotifications(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new Me.Notifications.Query(User.GetUserId()), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
    }
}