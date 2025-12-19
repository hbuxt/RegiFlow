namespace Api.Infrastructure.Localization
{
    public static class ErrorCodes
    {
        public const string RegisterEmailAlreadyExists = "REGISTER_EMAIL_ALREADY_EXISTS";
        public const string RegisterRoleNotFound = "REGISTER_ROLE_NOT_FOUND";
        public const string RegisterUnexpectedError = "REGISTER_UNEXPECTED_ERROR";
        public const string LoginInvalidCredentials = "LOGIN_INVALID_CREDENTIALS";
        public const string LoginUnexpectedError = "LOGIN_UNEXPECTED_ERROR";

        public const string UpdateMyDetailsUserNotFound = "UPDATEMYDETAILS_USER_NOT_FOUND";
        public const string UpdateMyDetailsUnexpectedError = "UPDATEMYDETAILS_UNEXPECTED_ERROR";
        public const string GetMyDetailsUserNotFound = "GETMYDETAILS_USER_NOT_FOUND";
        public const string DeleteMyDetailsUserNotFound = "DELETEMYDETAILS_USER_NOT_FOUND";
        public const string DeleteMyDetailsUnexpectedError = "DELETEMYDETAILS_UNEXPECTED_ERROR";
        public const string ListMyRolesUserNotFound = "LISTMYROLES_USER_NOT_FOUND";

        public const string GeneralUnexpectedError = "GENERAL_UNEXPECTED_ERROR";

        public const string EmailRequired = "EMAIL_REQUIRED";
        public const string EmailInvalid = "EMAIL_INVALID";
        public const string EmailTooLong = "EMAIL_TOO_LONG";
        public const string PasswordRequired = "PASSWORD_REQUIRED";
        public const string PasswordTooShort = "PASSWORD_TOO_SHORT";
        public const string PasswordTooLong = "PASSWORD_TOO_LONG";
        public const string PasswordMismatch = "PASSWORDS_MISMATCH";
        public const string ConfirmPasswordRequired = "CONFIRM_PASSWORD_REQUIRED";
        public const string FirstNameTooLong = "FIRST_NAME_TOO_LONG";
        public const string LastNameTooLong = "LAST_NAME_TOO_LONG";
    }
}