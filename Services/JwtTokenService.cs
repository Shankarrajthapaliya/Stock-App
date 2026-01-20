using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using web.Helper;
using web.Interface;

namespace web.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _Jwt ;
        private readonly UserManager<IdentityUser> _userManager ;

        public JwtTokenService(IOptions<JwtSettings> jwt , UserManager<IdentityUser> userManager)
        {
            _Jwt = jwt.Value;
            _userManager = userManager;
        }
        public async Task<(string token, DateTime UTCexpiration)> CreateJwtAsync(IdentityUser user)
        {
        
          List<Claim> claims = new List<Claim>
          {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),

            new Claim(ClaimTypes.NameIdentifier, user.Id),

            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
          };
          
          var userClaims = await _userManager.GetClaimsAsync(user);
          claims.AddRange(userClaims);     

          var roles = await _userManager.GetRolesAsync(user);

          foreach (var role in roles)
            {
                claims.Add(new Claim (ClaimTypes.Role, role));
            }

            

        var getBytes = Encoding.UTF8.GetBytes(_Jwt.Key); 
        var getSignKey = new SymmetricSecurityKey(getBytes);
        var creds = new SigningCredentials(getSignKey, SecurityAlgorithms.HmacSha256);
       
        var expiresatUTC = DateTime.UtcNow.AddMinutes(_Jwt.TokenMinutes);
         

         var tokenDescriptor = new JwtSecurityToken(
                issuer: _Jwt.Issuer,
                audience: _Jwt.Audience,
                claims: claims,
                expires: expiresatUTC,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);


           return (tokenString, expiresatUTC);
        }
    }
}