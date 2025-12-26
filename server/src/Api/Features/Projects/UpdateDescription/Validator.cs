using Api.Domain.Constants;
using FluentValidation;

namespace Api.Features.Projects.UpdateDescription
{
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            var descriptionTooLong = "Description is too long, it must not exceed 256 characters";
            
            RuleFor(c => c.Description)
                .MaximumLength(256).WithName(FieldNames.ProjectDescription).WithMessage(descriptionTooLong);
        }
    }
}