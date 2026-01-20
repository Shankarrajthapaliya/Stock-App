using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.Models;

namespace web.Controllers.DTO
{
    public class CreateStockDTO
    {
      [Required]
        [MinLength(1,ErrorMessage ="At least 1 character")]
        [MaxLength(5, ErrorMessage ="No more than 5 characters")]
     public string Symbol { get; set; } = string.Empty;
  [Required]
        [MinLength(3,ErrorMessage ="At least 3 characters")]
        [MaxLength(100, ErrorMessage ="No more than 100 characters")]
    public string CompanyName { get; set; } = string.Empty;
   [Required]
 
    public decimal Purchase { get; set; }
     [Required]
       
    public decimal LastDiv { get; set; }
    [Required]
     
    public long MarketCap { get; set; }
   [Required]
        [MinLength(1,ErrorMessage ="At least 20 characters")]
        [MaxLength(100, ErrorMessage ="No more than 2500 characters")]
    public string Industry { get; set; } = string.Empty ;
    }
}