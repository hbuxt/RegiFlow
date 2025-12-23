using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.ListMyPermissions
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "LISTMYPERMISSIONS_USERNOTFOUND";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "LISTMYPERMISSIONS_USERNOTAUTHORIZED";
            var message = "You don't have permission to view your permissions.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "LISTMYPERMISSIONS_UNEXPECTEDERROROCCURRED";
            var message = "An unexpected error occurred when retrieving your permissions. Please try again.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
    }
}