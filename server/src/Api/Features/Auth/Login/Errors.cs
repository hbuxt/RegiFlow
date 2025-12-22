using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Auth.Login
{
    public static class Errors
    {
        public static Error InvalidCredentials()
        {
            var code = "LOGIN_INVALIDCREDENTIALS";
            var message = "The email or password you entered does not match an existing account.";
            
            return new Error(ErrorStatus.NotFound, code, message);
        }

        public static Error SomethingWentWrong()
        {
            var code = "LOGIN_UNEXPECTEDERROR";
            var message = "An unexpected error occurred when signing you in. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}