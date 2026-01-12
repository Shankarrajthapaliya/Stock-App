using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Seed
{
    public class SeedOptions
    {
         public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;

        public string EmployeeEmail { get; set; } = string.Empty;
        public string EmployeePassword { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
         public string AdminOwnerEmail { get; set; } = string.Empty;
        public string AdminOwnerPassword { get; set; } = string.Empty;

        
    }
}