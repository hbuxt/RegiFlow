using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.Rename
{
    public static class Errors
    {
        public static Error ProjectNotFound()
        {
            var code = "RENAMEPROJECT_PROJECTNOTFOUND";
            var message = "We couldn't locate your project.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "RENAMEPROJECT_USERNOTAUTHORIZED";
            var message = "You don't have permission to rename a project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }

        public static Error DuplicateProjectName()
        {
            var code = "RENAMEPROJECT_DUPLICATENAME";
            var message = "Unable to rename project, a project with the same name already exists.";

            return new Error(ErrorStatus.Conflict, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "RENAMEPROJECT_UNEXPECTEDERROR";
            var message = "An unexpected error occurred when renaming your project. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}