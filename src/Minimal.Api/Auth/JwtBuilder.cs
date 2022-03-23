using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Minimal.Api.Extensions;
using Minimal.Api.Auth.Models;

namespace Minimal.Api.Auth
{
    public class JwtBuilder
    {
        private AppJwtSettings _appJwtSettings;

        public JwtBuilder(AppJwtSettings appJwtSettings)
        {
            _appJwtSettings = appJwtSettings ?? throw new ArgumentException(nameof(appJwtSettings));
        }

        public UserResponse BuildToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appJwtSettings.SecretKey ?? throw new ArgumentException("Secret Key cannot be null."));
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appJwtSettings.Issuer,
                Audience = _appJwtSettings.Audience,
                //Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appJwtSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            });

            var tokenWrited = tokenHandler.WriteToken(token);

            return BuildUserResponse(tokenWrited, user);
        }

        private UserResponse BuildUserResponse(string tokenWrited, IdentityUser user)
        {
            var userResponse = new UserResponse
            {
                AccessToken = tokenWrited,
                ExpiresIn = TimeSpan.FromHours(_appJwtSettings.Expiration).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email
                }
            };

            return userResponse;
        }
    }
}