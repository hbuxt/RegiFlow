using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.Rename
{
    public static class Errors
    {
        public static Error ProjectNotFound()
        {
            var code = "project_not_found";
            var message = "We couldn't locate your project when renaming it.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to rename a project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }

        public static Error DuplicateProjectName()
        {
            var code = "project_duplicate_name";
            var message = "Unable to create project, one of your other projects has the same name.";

            return new Error(ErrorStatus.Conflict, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "unexpected_error";
            var message = "An unexpected error occurred when renaming your project. Please try again.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}