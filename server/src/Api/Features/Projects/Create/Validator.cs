using Api.Domain.Constants;
using FluentValidation;

namespace Api.Features.Projects.Create
{
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            var nameRequired = "Name is required";
            var nameTooLong = "Name is too long, it must not exceed 64 characters";
            var descriptionTooLong = "Description is too long, it must not exceed 256 characters";
            
            RuleFor(c => c.Name)
                .NotNull().WithName(FieldNames.ProjectName).WithMessage(nameRequired)
                .NotEmpty().WithName(FieldNames.ProjectName).WithMessage(nameRequired)
                .MaximumLength(64).WithName(FieldNames.ProjectName).WithMessage(nameTooLong);

            RuleFor(c => c.Description)
                .MaximumLength(256).WithName(FieldNames.ProjectDescription).WithMessage(descriptionTooLong);
        }
    }
}