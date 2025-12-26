using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Features.Roles.ListByScope
{
    public static class Errors
    {
        public static Error UserNotAuthorized()
        {
            var code = "user_not_authorized";
            var message = "You don't have permission to view roles.";

            return new Error(ErrorStatus.Forbidden, code, message);
        }
    }
}