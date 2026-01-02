using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using web.DTO;
using web.Interface;
using web.Mappers;
using web.Models;
using web.Models.Data;

namespace web.Repo
{
    public class CommentRepository : ICommentRepo
    {
        private readonly ApplicationDBContext _context ;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context ;
        }


        public async Task<Comment?> CreateComment(CreateCommentDTO dto)
        {
            var stockSymbol = dto.Symbol ;
            var comment = dto.Creation();
            

          var stock =  await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == stockSymbol);

          if (stock ==null) return null; 
            comment.StockId = stock.Id ;
          _context.Comments.Add(comment);
            _context.SaveChanges();

            
            return comment ;
        }

        public async Task<List<CommentDTO>> GetAllComments()
        {
            var comments = await  _context.Comments.Include(s=> s.Stock).Select(s => s.toCommentDTO()).ToListAsync();

            return comments ;
        }

        public async Task<CommentDTO?> GetCommentByID(int id)
        {
            var comment = await _context.Comments.Include(s => s.Stock).FirstOrDefaultAsync(s => s.Id == id);
            if(comment==null) return null;
         
            return   comment.toCommentDTO();

        }
    }
}