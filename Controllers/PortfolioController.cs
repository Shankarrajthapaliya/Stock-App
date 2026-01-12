using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using web.DTO;
using web.Interface;
using web.Models.Data;

namespace web.Controllers
{

    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService ;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddStockToPortfolioAsync([FromBody]AddtoPortfolioDTO dTO)
        
        {
             if (!ModelState.IsValid){
                return BadRequest(ModelState);
                }
           var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

           if(userID == null)
            {
                return NotFound() ;
            }

        var stock =  await _portfolioService.AddStock(dTO,userID);
            return Ok(stock);
        } 
        

        
    }
}