using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WSFeed.Security
{
    public interface IJWTGenerator
    {
        string GenerateToken(string userId);
    }

    public class JWTGenerator : IJWTGenerator
    {
        private readonly JWTConfig _jwtconfig;

        public JWTGenerator(IOptions<JWTConfig> jwtConfig)
        {
            _jwtconfig = jwtConfig.Value ?? throw new ArgumentNullException(nameof(jwtConfig));
        }




        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretkey = Encoding.ASCII.GetBytes(_jwtconfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtconfig.ExpirationMinutes),
                Issuer = _jwtconfig.Issuer,
                Audience = _jwtconfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretkey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
