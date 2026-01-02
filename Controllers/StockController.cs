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
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var result = await _stockService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _stockService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _stockService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.toStockDTO());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateStockDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _stockService.UpdateAsync(id, dto);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var ok = await _stockService.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

    
