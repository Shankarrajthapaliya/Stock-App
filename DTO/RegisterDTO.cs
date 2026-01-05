using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTO
{
    public class RegisterDTO
    {
        [Required, EmailAddress]
        
        public required string Email {get; set;}
        [Required]
        public required string UserName {get; set;}
       [Required]
        public required string Password {get; set;} 
    }
}