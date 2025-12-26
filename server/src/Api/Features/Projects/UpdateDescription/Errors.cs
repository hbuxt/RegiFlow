using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.UpdateDescription
{
    public static class Errors
    {
        public static Error ProjectNotFound()
        {
            var code = "project_not_found";
            var message = "We couldn't locate your project.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to update this project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
    }
}