using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.DTO;
using web.Interface;
using web.Mappers;
using web.Models;

namespace web.Services
{
   public class CommentService : ICommentService
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IStockRepo _stockRepo;

        public CommentService(ICommentRepo commentRepo, IStockRepo stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            var comments = await _commentRepo.GetAllAsync();
            return comments.Select(c => c.toCommentDTO());
        }

        public async Task<CommentDTO?> GetByIdAsync(int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            return comment is null ? null : comment.toCommentDTO();
        }

        
        
        public async Task<Comment?> CreateAsync(CreateCommentDTO dto)
        {
            Comment comment = dto.Creation();

            // find stock by Symbol 
            var stock = await _stockRepo.GetBySymbolAsync(dto.Symbol);
            if (stock is null) return null;

            comment.StockId = stock.Id;

            var created = await _commentRepo.AddAsync(comment);
            return created;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _commentRepo.DeleteAsync(id);
        }
    }
}