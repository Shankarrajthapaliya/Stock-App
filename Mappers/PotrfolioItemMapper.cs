using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.DTO;
using web.Models;

namespace web.Mappers
{
    public static class PotrfolioItemMapper
    {
        public static PortfolioItemDTO toPortfolioDTO(this PortfolioItem portfolioItem)
        {
            return new PortfolioItemDTO
            {
                Username = portfolioItem.User.UserName,
                CompanyName = portfolioItem.stock.CompanyName,

                
            };
        }
    }
}