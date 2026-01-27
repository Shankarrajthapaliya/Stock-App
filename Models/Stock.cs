using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace web.Models
{
    public class Stock
    {

       [Key]
        public int Id{get; set;}

         public string Symbol{get; set;} = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        public long MarketCap{get; set;}

        public string Industry { get; set; } = string.Empty;

       public  List<Comment?> Comments {get; set;} = new List<Comment?>();
        
    }
}