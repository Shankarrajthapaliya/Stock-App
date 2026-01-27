using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTO
{
    public class CommentToStockDTO
    {
          public string Symbol { get; set; } = string.Empty;
  
    public string CompanyName { get; set; } = string.Empty;
    public long MarketCap { get; set; }
   
    public string Industry { get; set; } = string.Empty ;


    }
}