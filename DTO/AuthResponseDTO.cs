using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTO
{
    public class AuthResponseDTO
    {
        public string Username {get; set;} = string.Empty;

        public string Token{get; set;} = string.Empty;

        public DateTime Expiration {get; set;}
    }
}