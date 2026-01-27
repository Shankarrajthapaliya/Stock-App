using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.DTO;
using web.Models;

namespace web.Interface
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task<CommentDTO?> GetByIdAsync(int id);
        Task<Comment?> CreateAsync(CreateCommentDTO dto); // uses dto.Symbol
        Task<bool> DeleteAsync(int id);
        Task<List<CommentDTO>> GetBySymbolAsync(string symbol);
    }
}