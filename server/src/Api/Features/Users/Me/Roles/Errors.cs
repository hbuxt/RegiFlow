using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.Me.Roles
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "user_not_found";
            var message = "We couldn't locate your account when retrieving your roles.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to view your roles.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }

        public static Error SomethingWentWrong()
        {
            var code = "unexpected_error";
            var message = "An unexpected error occurred when retrieving your roles. Please try again.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}