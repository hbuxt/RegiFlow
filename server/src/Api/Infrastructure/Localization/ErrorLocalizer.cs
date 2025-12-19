using System.Collections.Generic;
using System.Globalization;

namespace Api.Infrastructure.Localization
{
    internal sealed class ErrorLocalizer : IErrorLocalizer
    {
        private readonly Dictionary<string, string> _errorMessages;
        
        public ErrorLocalizer()
        {
            _errorMessages = new Dictionary<string, string>()
            {
                { ErrorCodes.RegisterEmailAlreadyExists, "The email is already in use. Please use a different one or try logging in." },
                { ErrorCodes.RegisterRoleNotFound, "An unexpected error occurred when registering your new account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.RegisterUnexpectedError, "An unexpected error occurred when registering your new account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.LoginInvalidCredentials, "The email or password you entered does not match an existing account." },
                { ErrorCodes.LoginUnexpectedError, "An unexpected error occurred when signing you in. Please try again later or contact support if the problem persists." },

                { ErrorCodes.UpdateMyDetailsUserNotFound, "We couldn't locate your account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.UpdateMyDetailsUnexpectedError, "An unexpected error occurred when updating your details. Please try again later or contact support if the problem persists." },
                { ErrorCodes.GetMyDetailsUserNotFound, "We couldn't locate your account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.DeleteMyDetailsUserNotFound, "We couldn't locate your account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.DeleteMyDetailsUnexpectedError, "An unexpected error occurred when deleting your account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.ListMyRolesUserNotFound, "We couldn't locate your account. Please try again later or contact support if the problem persists." },
                { ErrorCodes.GeneralUnexpectedError, "An unexpected error occurred. Please try again later or contact support if the problem persists." },
                
                { ErrorCodes.EmailRequired, "Email is required" },
                { ErrorCodes.EmailInvalid, "Email is not a valid email" },
                { ErrorCodes.EmailTooLong, "Email is too long, it must not exceed {0} characters" },
                { ErrorCodes.PasswordRequired, "Password is required" },
                { ErrorCodes.PasswordTooShort, "Password is too short, it must be at least {0} characters" },
                { ErrorCodes.PasswordTooLong, "Password is too long, it must not exceed {0} characters" },
                { ErrorCodes.PasswordMismatch, "Passwords must match" },
                { ErrorCodes.ConfirmPasswordRequired, "Confirm password is required" },
                { ErrorCodes.FirstNameTooLong, "First name is too long, it must not exceed {0} characters" },
                { ErrorCodes.LastNameTooLong, "Last name is too long, it must not exceed {0} characters" }
            };
        }
        
        public string GetMessage(string key, CultureInfo? culture = null)
        {
            if (!_errorMessages.TryGetValue(key, out var message))
            {
                return _errorMessages[ErrorCodes.GeneralUnexpectedError];
            }

            return message;
        }
        
        public string GetMessage(string key, object arg, CultureInfo? culture = null)
        {
            if (!_errorMessages.TryGetValue(key, out var message))
            {
                return _errorMessages[ErrorCodes.GeneralUnexpectedError];
            }

            return string.Format(message, arg);
        }
    }
}