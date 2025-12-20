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

namespace Api.Features.Auth
{
    [ApiController]
    [Route(EndpointRoutes.AuthController)]
    [AllowAnonymous]
    public sealed class Controller : BaseController
    {
        public Controller(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route(EndpointRoutes.Register, Name = EndpointNames.Register)]
        [EnableRateLimiting(RateLimitPolicies.IpAddressFixedWindow)]
        [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Register.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Authentication)]
        public async Task<IResult> Register([FromForm] Register.Request? request, CancellationToken token)
        {
            var result = await _mediator.Send(new Register.Command(
                request?.Email,
                request?.Password,
                request?.ConfirmPassword), token);

            return result.Match(
                _ => Results.CreatedAtRoute(EndpointNames.Register, value: result.Value),
                _ => result.ToProblemDetails());
        }
        
        [HttpPost]
        [Route(EndpointRoutes.Login, Name = EndpointNames.Login)]
        [EnableRateLimiting(RateLimitPolicies.IpAddressFixedWindow)]
        [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(Login.Response))]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [Tags(EndpointTags.Authentication)]
        public async Task<IResult> Login([FromForm] Login.Request? request)
        {
            var result = await _mediator.Send(new Login.Command(
                request?.Email,
                request?.Password));

            return result.Match(
                _ => Results.Ok(result.Value),
                _ => result.ToProblemDetails());
        }
    }
}