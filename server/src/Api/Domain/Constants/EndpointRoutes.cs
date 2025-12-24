namespace Api.Domain.Constants
{
    public static class EndpointRoutes
    {
        public const string AuthController = "api/auth";
        public const string Register = "register";
        public const string Login = "login";

        public const string UsersController = "api/users";
        public const string GetMyDetails = "me";
        public const string UpdateMyDetails = "me";
        public const string DeleteMyDetails = "me";
        public const string ListMyRoles = "me/roles";
        public const string ListMyProjects = "me/projects";
        public const string ListMyPermissions = "me/permissions";

        public const string ProjectsController = "api/projects";
        public const string CreateProject = "";
        public const string GetProjectById = "{id}";
        public const string UpdateProject = "{id}";
        public const string RenameProject = "{id}/rename";
        public const string ListProjectPermissions = "{id}/permissions";
    }
}