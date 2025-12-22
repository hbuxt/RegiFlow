using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Application.Extensions;
using Api.Domain.Constants;
using Api.Domain.Enums;
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
    }
}