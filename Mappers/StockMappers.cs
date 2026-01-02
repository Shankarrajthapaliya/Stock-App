using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Controllers.DTO;
using web.DTO;
using web.Models;

namespace web.Mappers
{
    public static class StockMappers
    {
      

        public static StockDTO toStockDTO(this Stock stockModel)
        {
    return new StockDTO
    {
        Symbol = stockModel.Symbol,
        CompanyName = stockModel.CompanyName,
        Purchase = stockModel.Purchase,
        LastDiv = stockModel.LastDiv,
        MarketCap = stockModel.MarketCap,
        Industry = stockModel.Industry,

        Comments = stockModel.Comments
            .Select(c => new CommentSummaryDTO
            {
             
                Title = c.Title,
                Content = c.Content,
                CreatedOn = c.CreatedOn
                
            })
            .ToList()
    };
}

        public static Stock addedStock (this CreateStockDTO stockToAdd)
        {
            Stock addStock = new Stock();


            addStock.Symbol = stockToAdd.Symbol;
           
           addStock.CompanyName = stockToAdd.CompanyName;
             addStock.Purchase = stockToAdd.Purchase;
            addStock.LastDiv = stockToAdd.LastDiv;
            addStock.MarketCap = stockToAdd.MarketCap;
            addStock.Industry = stockToAdd.Industry;

            return addStock ;

        }

    }
}