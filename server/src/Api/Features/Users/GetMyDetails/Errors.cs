using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.GetMyDetails
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "user_not_found";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }

        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to view your details.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
    }
}