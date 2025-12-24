using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.Me.Permissions
{
    public static class Errors
    {
        public static Error ProjectNotFound()
        {
            var code = "project_not_found";
            var message = "We couldn't locate your project when retrieving your permissions.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to view your permissions in this project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "unexpected_error";
            var message = "An unexpected error occurred when retrieving your permissions. Please try again.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}