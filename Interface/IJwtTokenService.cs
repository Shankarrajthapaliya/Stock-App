using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace web.Interface
{
    public interface IJwtTokenService
    {
        public Task<(string token, DateTime UTCexpiration)> CreateJwtAsync(IdentityUser user) ;
    }
}