using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.GetById
{
    public static class Errors
    {
        public static Error ProjectNotFound()
        {
            var code = "GETPROJECTBYID_PROJECTNOTFOUND";
            var message = "We couldn't locate your project.";

            return new Error(ErrorStatus.NotFound, code, message);
        }

        public static Error UserNotAuthorized()
        {
            var code = "GETPROJECTBYID_USERNOTAUTHORIZED";
            var message = "You don't have permission to view this project.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
    }
}