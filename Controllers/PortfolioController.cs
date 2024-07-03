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
    }
}
