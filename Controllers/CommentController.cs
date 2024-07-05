using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Mappers;
using api.Dto;
using api.Interfaces;
using Microsoft.AspNetCore.Identity;
using api.Models;
using api.Extenstion;
namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :  ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICommentRepository _commentRepo;
        private IStockRepository _stockRepo;
        private readonly UserManager<User> _user;

        public CommentController(ApplicationDBContext context , ICommentRepository commentRepo , IStockRepository stockRepo , UserManager<User> user)
        {
            _commentRepo = commentRepo;
            _context = context;
            _stockRepo = stockRepo;
            _user = user;
        }


      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           
            var comments = await _commentRepo.GetAllCommentAsync();
            if(comments ==null){
                return NotFound();
        }
        return Ok(comments);
    }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
         var comment = await _commentRepo.GetByIdAsync(id);
         if(comment == null) {
            return NotFound();
         }
         return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute]int stockId , [FromBody] CreateCommentDto comment)
        {
             if(!ModelState.IsValid)
            return BadRequest(ModelState);
        if(!await _stockRepo.StockExist(stockId))
        {
            return BadRequest("Stock Does Not Exist");
        }
        var username = User.GetUserName();
        var user = _user.FindByNameAsync(username);
        if(user == null) {
            return NotFound("Not Found User");
        }
        var commentModel = comment.ToCommentFromCreate(stockId);
        commentModel.UserId = user.Id.ToString();
        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById) , new {id = commentModel.Id} , commentModel.ToCommentDto());
}
    [HttpPut]
    [Route("{commentId:int}")]
    public async Task<IActionResult> Update([FromRoute] int commentId , [FromBody] UpdateRequestComment comment)
    {
         if(!ModelState.IsValid)
            return BadRequest(ModelState);
     var commentModel = await _commentRepo.UpdateAsync(commentId , comment);
     if(commentModel == null) {
        return NotFound();
     }
     return Ok(commentModel);
    }
    [HttpDelete]
    [Route("{commentId:int}")]
    public async Task<IActionResult> Delete([FromRoute] int commentId)
    {
        var commentModel = await _commentRepo.DeleteAsync(commentId);
        if (commentModel == null) {
        return NotFound();
        }
    return NoContent();
    }
    }
}