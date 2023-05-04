using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicAppApi.Core.Constants;
using MusicAppApi.Models.Configurations;
using MusicAppApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Core.Interfaces;

namespace MusicAppApi.Core.Services
{
    public class JwtGenerator : IJWTGenerator
    {
        private readonly JWTConfiguration jwtConfiguration;

        public JwtGenerator(IOptions<JWTConfiguration> options)
        {
            this.jwtConfiguration = options.Value;
        }

        public string GenerateToken(string userId)
        {
            var authSigngingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.AccessTokenSecret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, AuthConstants.UserRoles.User),
                new Claim(ClaimTypes.Sid, userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfiguration.Issuer,
                audience: jwtConfiguration.Audience,
                expires: DateTime.Now.AddMinutes(jwtConfiguration.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigngingKey, SecurityAlgorithms.HmacSha256)
                );

            return tokenHandler.WriteToken(token);
        }
    }
}
