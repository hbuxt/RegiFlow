using Api.Application.Behaviours;

namespace Api.Features.Auth.Register
{
    public sealed record Command : ICommand<Response>
    {
        public Command(string? email, string? password, string? confirmPassword)
        {
            Email = email?.Trim() ?? string.Empty;
            Password = password ?? string.Empty;
            ConfirmPassword = confirmPassword ?? string.Empty;
        }

        public string Email { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
    }
}