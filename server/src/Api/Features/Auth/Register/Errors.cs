using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Auth.Register
{
    public static class Errors
    {
        public static Error AccountAlreadyExists()
        {
            var code = "REGISTER_EMAILALREADYEXISTS";
            var message = "The email is already in use. Please use a different one or try logging in.";

            return new Error(ErrorStatus.Conflict, code, message);
        }

        public static Error RoleNotFound()
        {
            var code = "REGISTER_ROLENOTFOUND";
            var message = "An unexpected error occurred when registering your new account. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }

        public static Error SomethingWentWrong()
        {
            var code = "REGISTER_UNEXPECTEDERROR";
            var message = "An unexpected error occurred when registering your new account. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}