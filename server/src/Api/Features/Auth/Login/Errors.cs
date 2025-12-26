using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Auth.Login
{
    public static class Errors
    {
        public static Error InvalidCredentials()
        {
            var code = "invalid_credentials";
            var message = "The email or password you entered does not match an existing account.";
            
            return new Error(ErrorStatus.NotFound, code, message);
        }
    }
}