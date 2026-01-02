using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.Helper;
using web.Interface;
using web.Mappers;
using web.Models;

namespace web.Services
{
     public class StockService : IStockService
    {
        private readonly IStockRepo _stockRepo;

        public StockService(IStockRepo stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<IEnumerable<StockDTO>> GetAllAsync(QueryObject query)
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            return stocks.Select(s => s.toStockDTO());
        }

        public async Task<StockDTO?> GetByIdAsync(int id)
        {
            var stock = await _stockRepo.GetById(id);
            return stock is null ? null : stock.toStockDTO();
        }

        public async Task<Stock> CreateAsync(CreateStockDTO dto)
        {
            var created = await _stockRepo.CreateStock(dto);
            return created;
        }

        public async Task<StockDTO?> UpdateAsync(int id, CreateStockDTO dto)
        {
            var updated = await _stockRepo.UpdateStock(id, dto);
            return updated is null ? null : updated.toStockDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _stockRepo.DeleteStockById(id);
        }

        
        
    }
}