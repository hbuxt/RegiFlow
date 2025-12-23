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
        
        public static Error SomethingWentWrong()
        {
            var code = "LISTMYPROJECTS_UNEXPECTEDERROROCCURRED";
            var message = "An unexpected error occurred when retrieving your projects. Please try again.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
    }
}