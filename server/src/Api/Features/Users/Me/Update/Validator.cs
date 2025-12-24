using Api.Domain.Constants;
using FluentValidation;

namespace Api.Features.Users.Me.Update
{
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            var firstNameTooLong = "First name is too long, it must not exceed 64 characters";
            var lastNameTooLong = "Last name is too long, it must not exceed 64 characters";
            
            RuleFor(c => c.FirstName)
                .MaximumLength(64).WithName(FieldNames.FirstName).WithMessage(firstNameTooLong);

            RuleFor(c => c.LastName)
                .MaximumLength(64).WithName(FieldNames.LastName).WithMessage(lastNameTooLong);
        }
    }
}