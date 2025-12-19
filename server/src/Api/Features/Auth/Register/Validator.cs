using Api.Domain.Constants;
using Api.Infrastructure.Localization;
using FluentValidation;

namespace Api.Features.Auth.Register
{
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IErrorLocalizer errorLocalizer)
        {
            var emailRequired = errorLocalizer.GetMessage(ErrorCodes.EmailRequired);
            var emailInvalid = errorLocalizer.GetMessage(ErrorCodes.EmailInvalid);
            var emailTooLong = errorLocalizer.GetMessage(ErrorCodes.EmailTooLong, ValidationLimits.EmailMaxLength);
            var passwordRequired = errorLocalizer.GetMessage(ErrorCodes.PasswordRequired);
            var passwordTooShort = errorLocalizer.GetMessage(ErrorCodes.PasswordTooShort, ValidationLimits.PasswordMinLength);
            var passwordTooLong = errorLocalizer.GetMessage(ErrorCodes.PasswordTooLong, ValidationLimits.PasswordMaxLength);
            var passwordMismatch = errorLocalizer.GetMessage(ErrorCodes.PasswordMismatch);
            var confirmPasswordRequired = errorLocalizer.GetMessage(ErrorCodes.ConfirmPasswordRequired);

            RuleFor(c => c.Email)
                .NotNull().WithName(FieldNames.Email).WithMessage(emailRequired)
                .NotEmpty().WithName(FieldNames.Email).WithMessage(emailRequired)
                .EmailAddress().WithName(FieldNames.Email).WithMessage(emailInvalid)
                .MaximumLength(ValidationLimits.EmailMaxLength).WithName(FieldNames.Email).WithMessage(emailTooLong);

            RuleFor(c => c.Password)
                .NotNull().WithName(FieldNames.Password).WithMessage(passwordRequired)
                .NotEmpty().WithName(FieldNames.Password).WithMessage(passwordRequired)
                .MinimumLength(ValidationLimits.PasswordMinLength).WithName(FieldNames.Password).WithMessage(passwordTooShort)
                .MaximumLength(ValidationLimits.PasswordMaxLength).WithName(FieldNames.Password).WithMessage(passwordTooLong)
                .Equal(c => c.ConfirmPassword).WithName(FieldNames.Password).WithMessage(passwordMismatch);

            RuleFor(c => c.ConfirmPassword)
                .NotNull().WithName(FieldNames.ConfirmPassword).WithMessage(confirmPasswordRequired);
        }
    }
}