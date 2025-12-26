using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.DeleteMyAccount
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "user_not_found";
            var message = "We couldn't locate your account when deleting your details.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to delete your details.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "unexpected_error";
            var message = "An unexpected error occurred when deleting your account. Please try again.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}