using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Npgsql.Replication.PgOutput.Messages;
using web.DTO;
using web.Interface;

namespace web.Controllers
{
    
    [ApiController]
    [Route("api/auth")]
     public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtToken ;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IJwtTokenService jwtToken, UserManager<IdentityUser> userManager)
        {
            _jwtToken = jwtToken;
            _userManager = userManager;
        }
   [HttpPost("register")]
  
        public async Task<IActionResult> Register(RegisterDTO dto)
        {

         if (!ModelState.IsValid){
               return BadRequest(ModelState);
         }
         var alreadyExists =  _userManager.FindByNameAsync(dto.UserName);
         if (alreadyExists == null)
            {
                return BadRequest(new { message = "Username already taken." });
            }
         var user = new IdentityUser();
         user.UserName = dto.UserName;
         user.Email = dto.Email;
         
        var result =  await _userManager.CreateAsync(user, dto.Password );
          if (!result.Succeeded)
            {
                
                return BadRequest(result.Errors.Select(e => e.Description));
            }
       
       return Ok(new { message = "User registered successfully." });

        }
      [HttpPost("login")]
        public async Task<IActionResult> Login(LogInDTO dto)
        {
                if (!ModelState.IsValid){
                return BadRequest(ModelState);
                }

            var user = await _userManager.FindByNameAsync(dto.Username);

        if(user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!checkPassword){
                return Unauthorized(new { message = "Invalid username or password." });
            }

           var (token, expires ) = await _jwtToken.CreateJwtAsync(user);
            
           var response = new AuthResponseDTO
           {
               Username = user.UserName ?? "",
               Token = token,
               Expiration = expires
               
           };

           return Ok(response);
           
                   }
    }
}