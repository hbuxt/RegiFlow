using Api.Domain.Constants;
using FluentValidation;

namespace Api.Features.Projects.InviteUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            var emailRequired = "Email is required";
            var emailInvalid = "Email is not a valid email";
            var emailTooLong = "Email is too long, it must not exceed 256 characters";
            var rolesRequired = "Roles are required";
            
            RuleFor(c => c.Email)
                .NotNull().WithName(FieldNames.Email).WithMessage(emailRequired)
                .NotEmpty().WithName(FieldNames.Email).WithMessage(emailRequired)
                .EmailAddress().WithName(FieldNames.Email).WithMessage(emailInvalid)
                .MaximumLength(256).WithName(FieldNames.Email).WithMessage(emailTooLong);

            RuleFor(c => c.Roles)
                .NotEmpty().WithName(FieldNames.Roles).WithMessage(rolesRequired);
        }
    }
}