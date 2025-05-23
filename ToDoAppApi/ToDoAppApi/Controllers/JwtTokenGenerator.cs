using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoAppApi.Models;

namespace ToDoAppApi.Controllers
{
    public class JwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;   
        }

        public string generateToken(User userData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, userData.Name),
                    new (ClaimTypes.Name, userData.Email),


                })
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var acceccToken = tokenHandler.WriteToken(securityToken);
            return acceccToken;
        }
    }
}
