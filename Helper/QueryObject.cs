using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace web.Helper
{
    public class QueryObject
    {
       
        [MinLength(1,ErrorMessage ="at least one character required")]
        [MaxLength(10, ErrorMessage ="Cant find Symbol")]
        public string? Symbol{get; set;}
         [MinLength(3,ErrorMessage ="at least three characters required")]
        [MaxLength(50, ErrorMessage ="Cant find Company")]
        public string? CompanyName{get; set;}
    }
}