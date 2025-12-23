using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.ListPermissionsByUser
{
    public static class Errors
    {
        public static Error ProjectNotFound()
        {
            var code = "LISTMYPERMISSIONS_PROJECTNOTFOUND";
            var message = "We couldn't locate your project.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "LISTMYPERMISSIONS_USERNOTAUTHORIZED";
            var message = "You don't have permission to view your permissions in project.";

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