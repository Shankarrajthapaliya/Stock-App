using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.DTO;
using web.Models;

namespace web.Interface
{
    public interface IPortfolioRepo
    {
        public Task<Stock> AddStock(AddtoPortfolioDTO dto, string userID);
    }
}