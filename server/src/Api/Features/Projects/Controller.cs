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

namespace Api.Features.Projects
{
    [ApiController]
    [Route(EndpointRoutes.ProjectsController)]
    [Authorize]
    public sealed class Controller : BaseController
    {
        public Controller(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route(EndpointRoutes.CreateProject, Name = EndpointNames.CreateProject)]
        [HasPermission(PermissionNames.CreateProjects)]
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
    }
}