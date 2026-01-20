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

        public string WorkerEmail { get; set; } = string.Empty;
        public string WorkerPassword { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
    }
}