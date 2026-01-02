using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.Helper;
using web.Models;

namespace web.Interface
{
   
        public interface IStockService
    {
        Task<IEnumerable<StockDTO>> GetAllAsync(QueryObject query);
        Task<StockDTO?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(CreateStockDTO dto);
        Task<StockDTO?> UpdateAsync(int id, CreateStockDTO dto);
        Task<bool> DeleteAsync(int id);
    }
    }
