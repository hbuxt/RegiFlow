using Api.Domain.Constants;
using FluentValidation;

namespace Api.Features.Auth.Register
{
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            var emailRequired = "Email is required";
            var emailInvalid = "Email is not a valid email";
            var emailTooLong = $"Email is too long, it must not exceed 256 characters";
            var passwordRequired = "Password is required";
            var passwordTooShort = $"Password is too short, it must be at least 8 characters";
            var passwordTooLong = $"Password is too long, it must not exceed 64 characters";
            var passwordMismatch = "Passwords must match";
            var confirmPasswordRequired = "Confirm password is required";

            RuleFor(c => c.Email)
                .NotNull().WithName(FieldNames.Email).WithMessage(emailRequired)
                .NotEmpty().WithName(FieldNames.Email).WithMessage(emailRequired)
                .EmailAddress().WithName(FieldNames.Email).WithMessage(emailInvalid)
                .MaximumLength(256).WithName(FieldNames.Email).WithMessage(emailTooLong);

            RuleFor(c => c.Password)
                .NotNull().WithName(FieldNames.Password).WithMessage(passwordRequired)
                .NotEmpty().WithName(FieldNames.Password).WithMessage(passwordRequired)
                .MinimumLength(8).WithName(FieldNames.Password).WithMessage(passwordTooShort)
                .MaximumLength(64).WithName(FieldNames.Password).WithMessage(passwordTooLong)
                .Equal(c => c.ConfirmPassword).WithName(FieldNames.Password).WithMessage(passwordMismatch);

            RuleFor(c => c.ConfirmPassword)
                .NotNull().WithName(FieldNames.ConfirmPassword).WithMessage(confirmPasswordRequired);
        }
    }
}