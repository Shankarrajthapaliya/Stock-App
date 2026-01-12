using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace web.Models
{
    public class PortfolioItem
    {
        [Key]
       public int Id{get; set;} 

       public string UserId{get; set;} = default!;
       public IdentityUser User {get; set;} = default!;


       public int StockID{get; set;} 
       public Stock stock {get; set;} = default! ;
       








       
    }
}