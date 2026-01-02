using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using web.Controllers.DTO;
using web.Helper;
using web.Interface;
using web.Mappers;
using web.Models;
using web.Models.Data;

namespace web.Repo
{
    public class StockRepository : IStockRepo
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStock(CreateStockDTO stockToCreate)
        {
            var stock = stockToCreate.addedStock();
           await _context.AddAsync(stock);
           await _context.SaveChangesAsync();

            return stock; 
        }

        public async Task<bool> DeleteStockById(int id)
        {
          var stocks = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stocks == null)
            {
                return false;
            }
            _context.Stocks.Remove(stocks);
           await _context.SaveChangesAsync();
           return true;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocksWithComments =  _context.Stocks.Include(s => s.Comments).AsQueryable();
           

            if(query.CompanyName != null){
            stocksWithComments = stocksWithComments.Where( s => s.CompanyName.Contains(query.CompanyName));
        
            }

            if(query.Symbol != null)
            {
                  stocksWithComments = stocksWithComments.Where( s => s.Symbol.Contains(query.Symbol));
            }

            return await stocksWithComments.ToListAsync() ;

        }

        public async Task<Stock?> GetById(int id)
        {
            var stockById = await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
            if (stockById == null)
            {
                return null ;
            }
            
            return stockById;
        }

        public async Task<IEnumerable<StockDTO>> GetByIndustry(string industry)
        {
              var getByIndustry = await _context.Stocks.Include(c => c.Comments).ToListAsync();
              var returnByIndustry = getByIndustry.Where(s => s.Industry == industry).Select(s => s.toStockDTO())  ;
       return returnByIndustry  ;
        }

        public async Task<Stock?> UpdateStock(int id, CreateStockDTO stockToUpdate)
        {
            var toUpdate = await _context.Stocks.FindAsync(id);
    if(toUpdate == null)
            {
                return null;
            }
            toUpdate.CompanyName = stockToUpdate.CompanyName;
            toUpdate.Industry = stockToUpdate.Industry;
            toUpdate.LastDiv = stockToUpdate.LastDiv;
            toUpdate.MarketCap = stockToUpdate.MarketCap;
            toUpdate.Purchase = stockToUpdate.Purchase;
            toUpdate.Symbol = stockToUpdate.Symbol;

            await _context.SaveChangesAsync();

         

           return toUpdate;

        


        }
    public async Task<Stock?> GetBySymbolAsync(string symbol)
{
    return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
}

            
        }
    }

