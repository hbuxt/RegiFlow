using Api.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Auth.Register
{
    public sealed record Request
    {
        public Request()
        {
        }

        [FromForm(Name = FieldNames.Email)]
        public string? Email { get; set; }

        [FromForm(Name = FieldNames.Password)]
        public string? Password { get; set; }

        [FromForm(Name = FieldNames.ConfirmPassword)]
        public string? ConfirmPassword { get; set; }
    }
}