using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.UpdateMyDetails
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "UPDATEMYDETAILS_USERNOTFOUND";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "UPDATEMYDETAILS_USERNOTAUTHORIZED";
            var message = "You don't have permission to update your details.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "UPDATEMYDETAILS_UNEXPECTEDERROR";
            var message = "An unexpected error occurred when updating your details. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}