using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Helper
{
    public class JwtSettings
    {
        public string Key {get; set;} = string.Empty;

        public string Issuer {get; set;} = string.Empty;

        public string Audience {get; set;} = string.Empty;

        public int TokenMinutes{get; set;} = 60;
    }
}