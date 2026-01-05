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
//this IOptions<jwtSettings> initializes value for class fields to the app.json values
        public JwtTokenService(IOptions<JwtSettings> jwt , UserManager<IdentityUser> userManager)
        {
            _Jwt = jwt.Value;
            _userManager = userManager;
        }
        public async Task<(string token, DateTime UTCexpiration)> CreateJwtAsync(IdentityUser user)
        {
            // what is .sub  and claims are i believe user roles and user info- username and id i understand- i guess this data is being 
            //brought from user and stored in claims - but what is sub and why it store user id.  the last .jti doesnt make sense at all
            
          List<Claim> claims = new List<Claim>
          {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),

            new Claim(ClaimTypes.NameIdentifier, user.Id),

            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // cant understand wtf is this guild and shit
          };

          var roles = await _userManager.GetRolesAsync(user);

          foreach (var role in roles)
            {
                claims.Add(new Claim (ClaimTypes.Role, role));
            }/// i see now we add roles. so how do we add the roles here? say we want 3 roles in this app. admin - worker and user- where 
            //do we wire this 

        var getBytes = Encoding.UTF8.GetBytes(_Jwt.Key); // the jwt key "make a key" from appsetting .json what are we getting bytes?
        var getSignKey = new SymmetricSecurityKey(getBytes);
        var creds = new SigningCredentials(getSignKey, SecurityAlgorithms.HmacSha256);
        // what is byte size and how does it generate key from "Key": "CHANGE_THIS_TO_A_LONG_RANDOM_SECRET_32+_CHARS",
        // then creds-  new Signingcredentials- is this a part of identity ? and its just passing the key and algorithm to apply?

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