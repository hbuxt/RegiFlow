using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.Create
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "user_not_found";
            var message = "We couldn't locate your account when creating your project.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to create a project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }

        public static Error DuplicateProjectName()
        {
            var code = "project_duplicate_name";
            var message = "Unable to create project, one of your other projects has the same name.";

            return new Error(ErrorStatus.Conflict, code, message);
        }
        
        public static Error RoleNotFound()
        {
            var code = "role_not_found";
            var message = "An unexpected error occurred when creating your project. Please try again.";

            return new Error(ErrorStatus.Failure, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "unexpected_error";
            var message = "An unexpected error occurred when creating your project. Please try again.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}