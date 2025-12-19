using System;
using System.Security.Claims;
using System.Text;
using Api.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Infrastructure.Identity
{
    internal sealed class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<JwtBearerOptions> _options;

        public TokenGenerator(IOptions<JwtBearerOptions> options)
        {
            _options = options;
        }
        
        public string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims:
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_options.Value.ExpiryInMinutes),
                SigningCredentials = credentials,
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.CreateToken(descriptor);

            return token;
        }
    }
}