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

namespace Api.Features.Roles
{
    [ApiController]
    [Route(EndpointRoutes.Roles)]
    [Authorize]
    public sealed class Controller : BaseController
    {
        public Controller(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        [Route(EndpointRoutes.ListRoles, Name = EndpointNames.ListRoles)]
        [EnableRateLimiting(RateLimitPolicies.UserTokenBucket)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ListByScope.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Users)]
        public async Task<IResult> ListMyRoles([FromQuery] ListByScope.Request? request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ListByScope.Query(
                User.GetUserId(),
                request?.Scope), cancellationToken);

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
    }
}