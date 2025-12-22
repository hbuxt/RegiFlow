using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.GetMyDetails
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "GETMYDETAILS_USERNOTFOUND";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }

        public static Error UserNotAuthorized()
        {
            var code = "GETMYDETAILS_USERNOTAUTHORIZED";
            var message = "You don't have permission to view your details.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
    }
}