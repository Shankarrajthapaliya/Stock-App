using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web.Models;

namespace web.DTO
{
    public class AddtoPortfolioDTO
    {
        [Required]
        [MinLength(1,ErrorMessage ="at least 1 character")]
        [MaxLength(5,ErrorMessage ="No more than five characters allowed")] 
        public required string Symbol{get; set;} 
    }
}