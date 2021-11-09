using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestBackend.Core.Models.Auth;
using RestBackend.Core.Models.Security;
using RestBackend.Core.Resources;
using RestBackend.Core.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RestBackend.Security
{
    public class JWTService : IJWTService
    {
        public JWTService(IOptions<JwtSettings> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public JwtSettings Options { get; }

        public TokenResource GenerateJwt(User user, IList<string> roles)
        {
            return GenerateJwt(user, roles, null);
        }

        public TokenResource GenerateJwt(User user, IList<string> roles, IList<Claim> permisions = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            if (permisions != null)
                claims.AddRange(permisions);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Options.ExpirationInDays));

            var tokenOptions = new JwtSecurityToken(
                issuer: Options.Issuer,
                audience: Options.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenResource
            {
                Token = tokenString,
                Expires = expires
            };
        }
    }
}
