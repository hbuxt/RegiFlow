using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Auth.Register
{
    public static class Errors
    {
        public static Error AccountAlreadyExists()
        {
            var code = "REGISTER_EMAIL_ALREADY_EXISTS";
            var message = "The email you provided is already in use. Please use a different email or try logging in.";

            return new Error(ErrorStatus.Conflict, code, message);
        }

        public static Error RoleNotFound()
        {
            var code = "REGISTER_UNEXPECTED_ERROR";
            var message = "An unexpected error occurred while registering your account. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }

        public static Error SomethingWentWrong()
        {
            var code = "REGISTER_UNEXPECTED_ERROR";
            var message = "An unexpected error occurred while registering your account. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}