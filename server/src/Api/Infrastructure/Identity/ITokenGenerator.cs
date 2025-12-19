using Api.Domain.Entities;

namespace Api.Infrastructure.Identity
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);
    }
}