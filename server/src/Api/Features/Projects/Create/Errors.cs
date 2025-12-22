using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.Create
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "CREATEPROJECT_USERNOTFOUND";
            var message = "We couldn't locate your account.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "CREATEPROJECT_USERNOTAUTHORIZED";
            var message = "You don't have permission to create a project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }

        public static Error DuplicateProjectName()
        {
            var code = "CREATEPROJECT_DUPLICATENAME";
            var message = "Unable to create project, an existing project with the same name already exists.";

            return new Error(ErrorStatus.Conflict, code, message);
        }
        
        public static Error RoleNotFound()
        {
            var code = "CREATEPROJECT_ROLENOTFOUND";
            var message = "An unexpected error occurred when creating your project. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
        
        public static Error SomethingWentWrong()
        {
            var code = "CREATEPROJECT_UNEXPECTEDERROR";
            var message = "An unexpected error occurred when creating your project. Please try again later.";

            return new Error(ErrorStatus.Failure, code, message);
        }
    }
}