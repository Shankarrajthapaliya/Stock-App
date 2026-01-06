using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using web.Controllers.DTO;

namespace web.Tests
{
    public class StockDTOTest
    {

       [Fact]
        public void CreateStockDTO_Invalid()
        {
            var dto = new CreateStockDTO()
            {
                Symbol = null,
                CompanyName = "Tesla",
                MarketCap = 2000000,
                LastDiv = 2,
                Purchase = 23,
                Industry = null

            };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(dto);


   bool isValid = Validator.TryValidateObject(dto, validationContext,validationResults);
   Assert.False(isValid);

        }
[Fact]
        public void CreateStockDTO_Valid()
        {
            var dto = new CreateStockDTO()
            {
                      Symbol = "TSLA",
                CompanyName = "Tesla",
                MarketCap = 2000000,
                LastDiv = 2,
                Purchase = 23,
                Industry = "Technology"
            };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto,validationContext,validationResults);
        Assert.True(isValid);
        }
    }
}