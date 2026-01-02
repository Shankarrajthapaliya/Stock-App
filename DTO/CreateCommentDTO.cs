using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTO
{
    public class CreateCommentDTO
    {

         [Required]
        [MinLength(5,ErrorMessage ="At least 5 characters")]
        [MaxLength(250, ErrorMessage ="No more than 250 characters")]
           public string Title {get; set;} = string.Empty;
[Required]
        [MinLength(20,ErrorMessage ="At least 20 characters")]
        [MaxLength(2500, ErrorMessage ="No more than 2500 characters")]
       public string Content {get; set;} = string.Empty;
[Required]
        [MinLength(1,ErrorMessage ="At least 1 character")]
        [MaxLength(5, ErrorMessage ="No more than 5 characters")]
        public string Symbol {get; set;} = string.Empty;
        
    }
}