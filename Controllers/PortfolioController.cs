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
using Microsoft.AspNetCore.Identity;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using api.Extenstion;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace api.Controllers
{
    [Route("api/Portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
   
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _user;

        private IPortfolioRepository _portfolioRepo;

        private readonly IStockRepository _stockRepo;



        
        public PortfolioController(UserManager<User> user , ApplicationDBContext context , IStockRepository stockRepo , IPortfolioRepository portfolioRepo)
        {
            _context = context;
            _user = user;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPortfolio()
        {
         var username = User.GetUserName();
         var user = await _user.FindByNameAsync(username);
         if(user == null) {
            return NotFound("Not Found User");
         }
         var portfolio = await _portfolioRepo.GetUserPortfolio(user);
         if(portfolio == null) {
            return NotFound();
         }
         return Ok(portfolio);


        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _user.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolasync(symbol);
            if (stock == null) return BadRequest("Not Found Stock");
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);
            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("can not add same stock to portfolio");
            }
            var portfolio = new Portfolio 
            {
                StockId = stock.Id,
                UserId = user.Id
            };
            await _portfolioRepo.CreatePortfolio(portfolio);
            return Ok(portfolio);
        }

        public async Task<IActionResult> DeletePortfo(string symbol)
        {
               var username = User.GetUserName();
            var user = await _user.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());
            if(filteredStock.Count() == 1)
            {
             await _portfolioRepo.DeletePortfolio(user ,symbol);
            } else {
                BadRequest("FilterStocked is not in your portfolio");
            }
            return Ok();
        }
    }
}
