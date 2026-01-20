using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.DTO;
using web.Models;

namespace web.Mappers
{
    public static  class CommentMapper
    {
        public static CommentDTO toCommentDTO(this Comment comment)
        {
            return new CommentDTO
            {
               Title = comment.Title,
               Content = comment.Content,
               CreatedOn = comment.CreatedOn,
               Stock = comment.Stock?.CommentToStock()
               
               

                
         
            };


        
        }

        public static Comment Creation( this CreateCommentDTO comment)
        {
          return  new Comment
            {
            Title = comment.Title,
            Content = comment.Content,
            
              
            };

        }
    }
}