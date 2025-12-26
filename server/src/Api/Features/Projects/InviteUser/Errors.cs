using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Projects.InviteUser
{
    public static class Errors
    {
        public static Error UserNotFound()
        {
            var code = "user_not_found";
            var message = "We couldn't locate the user when sending an invitation";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error RoleNotFound()
        {
            var code = "role_not_found";
            var message = "We couldn't locate an invitation role when sending an invitation.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error ProjectNotFound()
        {
            var code = "project_not_found";
            var message = "We couldn't locate the project when sending an invitation.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to invite users.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
        
        public static Error DuplicateUser()
        {
            var code = "project_duplicate_user";
            var message = "User is already present in the project.";

            return new Error(ErrorStatus.Conflict, code, message);
        }

        public static Error DuplicateInvitation()
        {
            var code = "project_duplicate_invitation";
            var message = "A pending invitation into this project already exists.";

            return new Error(ErrorStatus.Conflict, code, message);
        }
    }
}