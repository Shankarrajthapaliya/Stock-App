using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Npgsql.Internal;
using web.Controllers.DTO;
using web.Helper;
using web.Interface;
using web.Models;
using web.Services;

namespace web.Tests
{
    
    public class StockServiceTests
    {
        [Fact]
        public async Task  GetAllMethod_MustReturnAllStocks()
        {

            //Arrange all needed values for test
            var repoMock = new Mock<IStockRepo>();

            var query = new QueryObject
            {
                Symbol = null,
                CompanyName = null
            };

          var fakeStockRepo =      new List<Stock>
                {
                    new Stock
                    {
                    Id = 1,
                    CompanyName = "Apple Inc",
                    Symbol = "AAPL",
                    Industry = "Tech",
                    MarketCap = 1000,
                    Comments = new List<Comment?>()
                },
                new Stock
                {
                    Id = 2,
                    CompanyName = "Tesla",
                    Symbol = "TSLA",
                    Industry = "Auto",
                    MarketCap = 2000,
                    Comments = new List<Comment?>()
                }
                };


        repoMock.Setup(r => r.GetAllAsync(It.IsAny<QueryObject>()))
                .ReturnsAsync(fakeStockRepo);
    
    //CAll service
     var service = new StockService(repoMock.Object);
    var result =  await service.GetAllAsync(query);

    Assert.NotNull(result);
  var resultList = result.ToList();

  Assert.Equal(2, resultList.Count);


            Assert.Equal("Apple Inc", resultList[0].CompanyName);
            Assert.Equal("AAPL", resultList[0].Symbol);

            Assert.Equal("Tesla", resultList[1].CompanyName);
            Assert.Equal("TSLA", resultList[1].Symbol);

 repoMock.Verify(r => r.GetAllAsync(It.IsAny<QueryObject>()), Times.Once);
repoMock.VerifyNoOtherCalls();

        }
[Fact]
    public async Task Create_returnsStock()
        {
            var repoMock = new Mock<IStockRepo>(); 

            var dto = new CreateStockDTO
            {
                Symbol = "SA",
                CompanyName = "Surya",
                MarketCap = 200000,
                Industry = "Food"
                
            };
        var stockToReturn = new Stock
        {
            Id = 99,
              CompanyName = dto.CompanyName,
                Symbol = dto.Symbol,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap,
                Comments = new List<Comment?>()
        };
      

      repoMock.Setup(r => r.CreateStock(It.IsAny<CreateStockDTO>())).ReturnsAsync(stockToReturn);

      var service = new StockService(repoMock.Object);

      var result = await service.CreateAsync(dto);


            Assert.NotNull(result);
            Assert.Equal(99, result.Id);
            Assert.Equal("Surya", result.CompanyName);
            Assert.Equal("SA", result.Symbol);


  repoMock.Verify(r => r.CreateStock(It.IsAny<CreateStockDTO>()), Times.Once);

        }


      };

}