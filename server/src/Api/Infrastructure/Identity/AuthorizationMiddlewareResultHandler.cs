using System.Net.Mime;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Api.Infrastructure.Identity
{
    internal sealed class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        public AuthorizationMiddlewareResultHandler()
        {
        }
        
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = MediaTypeNames.Application.ProblemJson;

                var result = Result.Failure(new Error(ErrorStatus.TooManyRequests));

                await result.ToProblemDetails().ExecuteAsync(context);
                return;
            }

            await next(context);
        }
    }
}