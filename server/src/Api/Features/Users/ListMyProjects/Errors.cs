using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.ListMyProjects
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "LISTMYPROJECTS_USERNOTFOUND";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "LISTMYPROJECTS_USERNOTAUTHORIZED";
            var message = "You don't have permission to view your projects.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
    }
}