using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Users.RespondToMyInvitation
{
    public static class Errors
    {
        public static Error InvitationNotFound()
        {
            var code = "invitation_not_found";
            var message = "We couldn't locate a pending invitation.";

            return new Error(ErrorStatus.NotFound, code, message);
        }
        
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to respond to invitations.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }

        public static Error InvitationHasExpired()
        {
            var code = "invitation_expired";
            var message = "Unable to respond to invitation, invitation has expired.";

            return new Error(ErrorStatus.BadRequest, code, message);
        }
    }
}