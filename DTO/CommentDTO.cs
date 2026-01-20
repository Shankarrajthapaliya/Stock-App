using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.Models;

namespace web.DTO
{
    public class CommentDTO
    {
       
        public string Title {get; set;} = string.Empty;
        
       
       public string Content {get; set;} = string.Empty;
       
        public CommentToStockDTO? Stock {get; set;}
        
        public DateTime CreatedOn {get; set;}


    }
}