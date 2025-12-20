using System.Net.Mime;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(GetMyDetails.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> GetMyDetails()
        {
            var result = await _mediator.Send(new GetMyDetails.Query(HttpContext.User.GetUserId()));

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListMyRoles, Name = EndpointNames.ListMyRoles)]
        [HasPermission(PermissionNames.ViewMyRoles)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ListMyRoles.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyRoles()
        {
            var result = await _mediator.Send(new ListMyRoles.Query(HttpContext.User.GetUserId()));

            return result.Match(
                onSuccess => Results.Ok(result.Value),
                onFailure => result.ToProblemDetails());
        }
    }
}