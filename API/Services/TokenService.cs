using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        }

        /// <inheritdoc/>
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() => tokenHandler.CreateToken(tokenDescriptor));

            return tokenHandler.WriteToken(token);
        }

        /// <inheritdoc/>
        public async Task<string> GetUsernameFromAuthHeader(string authorizationHeader)
        {
            var strippedJwt = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() => handler.ReadJwtToken(strippedJwt));
            var senderUsername = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

            return senderUsername;
        }
    }
}