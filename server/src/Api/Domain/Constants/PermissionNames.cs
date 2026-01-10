namespace Api.Domain.Constants
{
    public static class PermissionNames
    {
        public const string UserRead = "user.read";
        public const string UserProfileUpdate = "user.profile.update";
        public const string UserDelete = "user.delete";
        public const string UserInvitationsUpdate = "user.invitations.update";
        public const string UserRolesRead = "user.roles.read";
        public const string UserPermissionsRead = "user.permissions.read";
        public const string UserNotificationsRead = "user.notifications.read";

        public const string RolesRead = "roles.read";

        public const string ProjectCreate = "project.create";
        public const string ProjectRead = "project.read";
        public const string ProjectNameUpdate = "project.name.update";
        public const string ProjectDescriptionUpdate = "project.description.update";
        public const string ProjectDelete = "project.delete";
        public const string ProjectUsersRead = "project.users.read";
        public const string ProjectUsersUpdate = "project.users.update";
        public const string ProjectPermissionsRead = "project.permissions.read";
        public const string ProjectInvitationsInvite = "project.invitations.invite";
        public const string ProjectInvitationsRevoke = "project.invitations.revoke";
    }
}