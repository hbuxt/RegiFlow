namespace Api.Domain.Constants
{
    public static class PermissionNames
    {
        public const string UserRead = "user.read";
        public const string UserUpdate = "user.update";
        public const string UserDelete = "user.delete";
        public const string UserRolesRead = "user.roles.read";
        public const string UserPermissionsRead = "user.permissions.read";

        public const string NotificationsRead = "notifications.read";
        public const string NotificationsUpdate = "notifications.update";

        public const string RolesRead = "roles.read";

        public const string ProjectCreate = "project.create";
        public const string ProjectRead = "project.read";
        public const string ProjectUpdate = "project.update";
        public const string ProjectDelete = "project.delete";
        public const string ProjectUsersRead = "project.users.read";
        public const string ProjectUsersUpdate = "project.users.update";
        public const string ProjectPermissionsRead = "project.permissions.read";
        public const string ProjectInvitationsRead = "project.invitations.read";
        public const string ProjectInvitationsInvite = "project.invitations.invite";
        public const string ProjectInvitationsRevoke = "project.invitations.revoke";
    }
}