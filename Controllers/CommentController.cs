using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.DTO;
using web.Interface;
using web.Mappers;
using web.Models;

namespace web.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _commentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _commentService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _commentService.CreateAsync(dto);
            if (created is null) return NotFound("Stock symbol not found.");

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.toCommentDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var ok = await _commentService.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
       
       [HttpGet("symbol/{symbol}")]
        public async Task<IActionResult> FindBySymbol(string symbol)
        {
            var comment = await _commentService.GetBySymbolAsync(symbol) ;
            return Ok(comment);
        }
       
    }
}