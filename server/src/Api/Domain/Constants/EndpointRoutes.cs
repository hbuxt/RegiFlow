namespace Api.Domain.Constants
{
    public static class EndpointRoutes
    {
        public const string Auth = "api/auth";
        public const string Register = "register";
        public const string Login = "login";

        public const string Users = "api/users";
        public const string GetMyDetails = "me";
        public const string UpdateMyDetails = "me";
        public const string DeleteMyAccount = "me";
        public const string ListMyRoles = "me/roles";
        public const string ListMyProjects = "me/projects";
        public const string ListMyPermissions = "me/permissions";

        public const string Roles = "api/roles";
        public const string ListRoles = "";

        public const string Projects = "api/projects";
        public const string CreateProject = "";
        public const string GetProjectById = "{id}";
        public const string UpdateProject = "{id}";
        public const string RenameProject = "{id}/rename";
        public const string DeleteProject = "{id}";
        public const string ListUsersInProject = "{id}/users";
        public const string ListMyPermissionsInProject = "{id}/users/me/permissions";
    }
}