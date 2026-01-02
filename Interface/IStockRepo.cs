using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.Helper;
using web.Models;

namespace web.Interface
{
    public interface IStockRepo
    {
        public  Task<List<Stock>> GetAllAsync(QueryObject query);

        public Task<StockDTO?> GetById(int id);

        public Task<IEnumerable<StockDTO>> GetByIndustry(string industry);

        public Task<Stock> CreateStock(CreateStockDTO stockToCreate);

        public Task<StockDTO?> UpdateStock(int id, CreateStockDTO stockToUpdate);

         public  Task<string?> DeleteStockById(int id);
      
    }
}