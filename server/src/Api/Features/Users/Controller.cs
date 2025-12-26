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
        [ProducesDefaultResponseType(typeof(GetMyDetails.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> GetMyDetails(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMyDetails.Query(User.GetUserId()), cancellationToken);

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
        [ProducesDefaultResponseType(typeof(UpdateMyProfile.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> UpdateMyProfile([FromBody] UpdateMyProfile.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateMyProfile.Command(
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
            var result = await _mediator.Send(new DeleteMyAccount.Command(User.GetUserId()), cancellationToken);

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
        [ProducesDefaultResponseType(typeof(ListMyRoles.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListMyRoles.Query(User.GetUserId()), cancellationToken);

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
        [ProducesDefaultResponseType(typeof(ListMyProjects.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyProjects(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListMyProjects.Query(User.GetUserId()), cancellationToken);
            
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
        [ProducesDefaultResponseType(typeof(ListMyPermissions.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyPermissions(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListMyPermissions.Query(User.GetUserId()), cancellationToken);
            
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
        [ProducesDefaultResponseType(typeof(ListMyNotifications.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyNotifications(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListMyNotifications.Query(User.GetUserId()), cancellationToken);
            
            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
    }
}