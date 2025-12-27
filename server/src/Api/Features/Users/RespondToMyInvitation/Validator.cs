using System;
using Api.Domain.Constants;
using FluentValidation;

namespace Api.Features.Users.RespondToMyInvitation
{
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            var responses = string.Join(", ", Enum.GetNames<InvitationStatus>());
            var responseRequired = $"A response of {responses} is required.";
            var responseInvalid = $"An invitation can only be {responses}";
            
            RuleFor(c => c.Status)
                .NotNull().WithName(FieldNames.InvitationStatus).WithMessage(responseRequired)
                .NotEmpty().WithName(FieldNames.InvitationStatus).WithMessage(responseRequired)
                .Must(r => Enum.TryParse<InvitationStatus>(r, ignoreCase: true, out _))
                .WithName(FieldNames.InvitationStatus).WithMessage(responseInvalid);

        }
    }
}