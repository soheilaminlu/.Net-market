using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{

    public class PortfolioRepository : IPortfolioRepository
    {
            private readonly ApplicationDBContext _context;
          
            public PortfolioRepository(ApplicationDBContext context , UserManager<User> user)
            {
                _context = context;;
            }

            public async Task<List<Stock>> GetUserPortfolio(User user)
            {
              return await _context.Portfilios.Where(u => u.UserId == user.Id)
              .Select(stock => new Stock
              {
              CompanyName = stock.Stock.CompanyName,
              Purchase = stock.Stock.Purchase,
              Symbol = stock.Stock.Symbol,
              Industry = stock.Stock.Industry,
              MarketCap = stock.Stock.MarketCap,
              LastDiv = stock.Stock.LastDiv
              }
              ).ToListAsync();
              
            }

            public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
            {
               await _context.Portfilios.AddAsync(portfolio);
               await _context.SaveChangesAsync();
               return portfolio;
               
            }
          
    }
}