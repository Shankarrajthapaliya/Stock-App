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
        private readonly ICommentRepo _commentRepo ;

        public CommentController(ICommentRepo commentRepo)
        {
            _commentRepo = commentRepo ;
        }
       
       [HttpGet]
       public async Task<IActionResult> GetAll()
        {
          var comments =   await _commentRepo.GetAllComments();

          return Ok(comments);
        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetById( int id)
        {
           var result =  await _commentRepo.GetCommentByID(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDTO dTOtoCreate)
        {

            if (!ModelState.IsValid)
    {
         return BadRequest(ModelState);
    }
          var newComment = await _commentRepo.CreateComment(dTOtoCreate);
           if (newComment == null) { return NotFound();}

           return CreatedAtAction(nameof(GetById), new {id = newComment.Id}, newComment.toCommentDTO());



        }
}
}