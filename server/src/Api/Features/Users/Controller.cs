using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Features.Users
{
    [ApiController]
    [Route(EndpointRoutes.UsersController)]
    [Authorize]
    public sealed class Controller : BaseController
    {
        public Controller(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        [Route(EndpointRoutes.GetMyDetails, Name = EndpointNames.GetMyDetails)]
        [HasPermission(PermissionNames.ViewMyDetails)]
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
        [HasPermission(PermissionNames.UpdateMyDetails)]
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
        [ProducesDefaultResponseType(typeof(UpdateMyDetails.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> UpdateMyDetails([FromBody] UpdateMyDetails.Request? request)
        {
            var result = await _mediator.Send(new UpdateMyDetails.Command(
                User.GetUserId(),
                request?.FirstName,
                request?.LastName));

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpDelete]
        [Route(EndpointRoutes.DeleteMyDetails, Name = EndpointNames.DeleteMyDetails)]
        [HasPermission(PermissionNames.DeleteMyDetails)]
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
        public async Task<IResult> DeleteMyDetails(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteMyDetails.Command(User.GetUserId()), cancellationToken);

            return result.Match(
                Results.NoContent,
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListMyRoles, Name = EndpointNames.ListMyRoles)]
        [HasPermission(PermissionNames.ViewMyRoles)]
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
    }
}