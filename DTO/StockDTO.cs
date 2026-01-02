using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web.DTO;

namespace web.Controllers.DTO
{
    public class StockDTO
    {
      
     public string Symbol { get; set; } = string.Empty;
  
    public string CompanyName { get; set; } = string.Empty;
   
    public decimal Purchase { get; set; }
     
    public decimal LastDiv { get; set; }
    
    public long MarketCap { get; set; }
   
    public string Industry { get; set; } = string.Empty ;

      public List<CommentSummaryDTO> Comments { get; set; } = new();
    }
}