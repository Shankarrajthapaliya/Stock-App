using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using web.DTO;
using web.Models;

namespace web.Interface
{
   
    public interface ICommentRepo 
    {
        public Task<List<CommentDTO>> GetAllComments();

        public Task<Comment?> CreateComment(CreateCommentDTO dto);

        public Task<CommentDTO?> GetCommentByID(int id);
    }
}