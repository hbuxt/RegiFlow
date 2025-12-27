namespace Api.Domain.Constants
{
    public static class FieldNames
    {
        public const string ErrorCode = "error_code";
        public const string ErrorMessage = "error_message";

        public const string Users = "users";
        public const string UserId = "id";
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string Email = "email";
        public const string Password = "password";
        public const string ConfirmPassword = "confirm_password";
        public const string AccessToken = "access_token";

        public const string Notifications = "notifications";
        public const string NotificationId = "id";
        public const string NotificationType = "type";
        public const string NotificationStatus = "status";
        public const string NotificationContent = "content";
        public const string NotificationCreatedAt = "created_at";
        public const string Invitation = "invitation";
        public const string InvitationToken = "token";
        public const string InvitationRegarding = "regarding";
        public const string InvitationExpiresAt = "expires_at";
        public const string InvitationStatus = "status";

        public const string Roles = "roles";
        public const string RoleId = "id";
        public const string RoleName = "name";
        public const string RoleScope = "scope";

        public const string Permissions = "permissions";

        public const string Projects = "projects";
        public const string ProjectId = "id";
        public const string ProjectName = "name";
        public const string ProjectDescription = "description";
        public const string ProjectCreatedAt = "created_at";
        public const string ProjectCreatedBy = "created_by";
    }
}