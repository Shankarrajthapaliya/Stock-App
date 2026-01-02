using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using web.Controllers.DTO;
using web.Helper;
using web.Interface;
using web.Mappers;
using web.Models;
using web.Models.Data;

namespace web.Controllers
{

[ApiController]
[Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
         private readonly IStockRepo _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepo stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) 
        {


             var stocks = await _stockRepo.GetAllAsync(query);
           var  stockWithComments = stocks.Select(s => s.toStockDTO());
          
         return Ok(stockWithComments);

            
            
            
        }


        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetById(id);

            if (stock == null) return NotFound();

            return Ok(stock);
         }
        
        

         [HttpGet("industry/{industry}")]
        public async Task<IActionResult> getByIndustry(string industry)
        {
        
         var getByIndustry = await _context.Stocks.ToListAsync();
         var returnIndustry = getByIndustry.Where(s => s.Industry == industry).Select(s => s.toStockDTO());
        
        
                

    return Ok(returnIndustry);
            
            
        }

      [HttpPost]
      public async Task<IActionResult> Create([FromBody] CreateStockDTO dto)
    {
      if (!ModelState.IsValid)
    {
         return BadRequest(ModelState);
    }

    var stockToAdd = await _stockRepo.CreateStock(dto);
       
return CreatedAtAction(nameof(GetById), new{id = stockToAdd.Id}, stockToAdd.toStockDTO());

    }


 [HttpPut]
 [Route("{id:int}")]
 public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] CreateStockDTO stock)
        {
            
           var dto = await _stockRepo.UpdateStock(id, stock);

           return Ok(dto);



        }   


        

[HttpDelete("{id:int}")]

public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var deleted = await _stockRepo.DeleteStockById(id);

          if (deleted != null)
            {
               return Ok(new {message = "Stock is deleted"}); 
            }


      else
      {
        return NotFound();
      }
        }


// [HttpDelete("delete/{symbol}")]
// public IActionResult DeleteBySymbol(string symbol)
//         {
//             var  stockToList= _context.Stocks.Where(s => s.Symbol == symbol)
//                                              .Select(s => s);

//             if(stockToList == null)
//             {
//                 return NotFound();
//             }

//             _context.Stocks.RemoveRange(stockToList);
//                  return Ok(new {message = "Stock is deleted"});

//         }


    
}

    }
