using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using web.DTO;
using web.Interface;
using web.Models;
using web.Models.Data;

namespace web.Repo
{
    public class PortfolioRepository : IPortfolioRepo
    {
       private readonly  ApplicationDBContext _context ;

         public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context ;
        }
        public async Task<Stock> AddStock(AddtoPortfolioDTO dto, string userID)
        {
             var stock =  await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == dto.Symbol);

           if (stock == null)
            {
                throw new InvalidOperationException("Stock not found");
            }
             
             
         _context.Portfolios.Add(new PortfolioItem
             {
                 UserId = userID,
                 StockID = stock.Id

             });
           await   _context.SaveChangesAsync() ;
             
             return stock ;
             
        }

        public async Task<List<PortfolioItem>> GetPortfolio(string id)
        {
           var item =  await _context.Portfolios.Where(x => x.UserId == id).Include(x => x.stock).Include(x => x.User).OrderByDescending(x => x.StockID).ToListAsync();

            return item;
        }
    }
}