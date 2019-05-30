using Microsoft.IdentityModel.Tokens;
using System;
using Root.Model;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Configuration
{
    public static class JwtTokenBuilder
    {
        public static string BuildToken(this User user, List<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            foreach(var role in roles){
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecrectKeyAhihiBleBle"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: "Test", audience: "Test", claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
