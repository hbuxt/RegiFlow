using Api.Application.Behaviours;

namespace Api.Features.Auth.Login
{
    public sealed record Command : ICommand<Response>
    {
        public Command(
            string? email,
            string? password)
        {
            Email = email?.Trim() ?? string.Empty;
            Password = password ?? string.Empty;
        }

        public string Email { get; init; }
        public string Password { get; init; }
    }
}