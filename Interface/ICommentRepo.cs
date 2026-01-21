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
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> AddAsync(Comment comment);
        Task<bool> DeleteAsync(int id);
      
        Task<List<CommentDTO>> GetBySymbol(string symbol);
    }
}