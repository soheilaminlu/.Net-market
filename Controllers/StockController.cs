using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Repository;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context , IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto())
            .ToList();
            if(stocks.Count == 0)
            {
                return NotFound("Not Found Stocks");
            }
            return Ok(stocks);
        }
        [HttpGet("{id:int}")]
        public async  Task<IActionResult> GetStock([FromRoute] int id)
        {
         var stock = await _stockRepo.GetStockByIdAsync(id);
         if(stock == null) {
            return NotFound("Not Found Stock");
         }
         return Ok(stock);
        }
        [HttpPost]

        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequest stockDto)
        {
             if(!ModelState.IsValid)
            return BadRequest(ModelState);
          var stockModel = stockDto.ToStockFromCreateDto();
      await _stockRepo.CreateStockAsync(stockModel);
          return CreatedAtAction(nameof(GetStock) , new {id = stockModel.Id} , stockModel.ToStockDto());
        }
         
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequest  updateDto)
        {
             if(!ModelState.IsValid)
            return BadRequest(ModelState);
        var stockModel = await _stockRepo.UpdateStockAsync(id , updateDto);
        if(stockModel == null) {
        return NotFound();
        };
         return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
    public async Task<IActionResult> DeleteStock([FromRoute] int id)
    {
        
        var stockModel = await _stockRepo.DeleteStockAsync(id);
        if(stockModel == null)
        {
            return NotFound();
        }
       return NoContent();
    }
    }
}