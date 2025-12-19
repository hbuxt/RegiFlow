using System;
using System.Collections.Generic;
using Api.Application.Abstractions;
using Api.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Api.Infrastructure.Extensions
{
    internal static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (!result.IsFailure)
            {
                throw new InvalidOperationException("This result succeeded," +
                    "but you attempted to create a problem details response." +
                    "This should only be done if the result has failed.");
            }

            if (result.Error == null)
            {
                throw new NullReferenceException("This result failed," +
                    "but the error was null when creating a problem details response." +
                    "Ensure you provide an error when a result fails to avoid this exception.");
            }

            return Results.Problem(
                statusCode: GetStatusCode(result.Error.Status),
                title: GetTitle(result.Error.Status),
                detail: GetDetail(result.Error.Status),
                type: GetType(result.Error.Status),
                extensions: result.Error.Errors == null ? null : new Dictionary<string, object?>()
                {
                    { "errors", result.Error.Errors }
                });
        }

        private static int GetStatusCode(ErrorStatus status)
        {
            return status switch
            {
                ErrorStatus.BadRequest => 400,
                ErrorStatus.Unauthorized => 401,
                ErrorStatus.Forbidden => 403,
                ErrorStatus.NotFound => 404,
                ErrorStatus.Conflict => 409,
                ErrorStatus.TooManyRequests => 429,
                _ => 500
            };
        }

        private static string GetTitle(ErrorStatus status)
        {
            return status switch
            {
                ErrorStatus.BadRequest => "Bad Request",
                ErrorStatus.Unauthorized => "Unauthorized",
                ErrorStatus.Forbidden => "Forbidden",
                ErrorStatus.NotFound => "Not Found",
                ErrorStatus.Conflict => "Conflict",
                ErrorStatus.TooManyRequests => "Too Many Requests",
                _ => "Server Error"
            };
        }

        private static string GetDetail(ErrorStatus status)
        {
            return status switch
            {
                ErrorStatus.BadRequest => "The request could not be understood or was missing required parameters.",
                ErrorStatus.Unauthorized => "Authentication failed or user does not have permissions for the requested operation.",
                ErrorStatus.Forbidden => "You do not have permission to access this resource.",
                ErrorStatus.NotFound => "The resource you are looking for could not be found.",
                ErrorStatus.Conflict => "There was a conflict with the current state of the resource, such as a duplicate entry.",
                ErrorStatus.TooManyRequests => "You have sent too many requests. Please try again later.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

        private static string GetType(ErrorStatus status)
        {
            return status switch
            {
                ErrorStatus.BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorStatus.Unauthorized => "https://tools.ietf.org/html/rfc7231#section-6.5.2",
                ErrorStatus.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                ErrorStatus.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorStatus.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                ErrorStatus.TooManyRequests => "https://datatracker.ietf.org/doc/html/rfc6585#section-4",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
        }
    }
}