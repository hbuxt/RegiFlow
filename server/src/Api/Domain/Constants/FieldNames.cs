namespace Api.Domain.Constants
{
    public static class FieldNames
    {
        public const string ErrorCode = "error_code";
        public const string ErrorMessage = "error_message";

        public const string UserId = "id";
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string Email = "email";
        public const string Password = "password";
        public const string ConfirmPassword = "confirm_password";
        public const string AccessToken = "access_token";

        public const string Roles = "roles";
        public const string RoleId = "id";
        public const string RoleName = "name";

        public const string Permissions = "permissions";

        public const string Projects = "projects";
        public const string ProjectId = "id";
        public const string ProjectName = "name";
        public const string ProjectDescription = "description";
        public const string ProjectCreatedAt = "created_at";
        public const string ProjectCreatedBy = "created_by";
    }
}