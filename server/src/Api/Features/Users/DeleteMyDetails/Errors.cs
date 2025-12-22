using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.DeleteMyDetails
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "DELETEMYDETAILS_USERNOTFOUND";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "DELETEMYDETAILS_USERNOTAUTHORIZED";
            var message = "You don't have permission to delete your details.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "DELETEMYDETAILS_UNEXPECTEDERROR";
            var message = "An unexpected error occurred when deleting your account. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}