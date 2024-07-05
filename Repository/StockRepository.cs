using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null)
            {
                return null;
            }
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.Comments).ThenInclude(c => c.User).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

            }

            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
           }
           if(!string.IsNullOrWhiteSpace(query.SortBy))
           {
            if(query.SortBy.Equals("Symbol" , StringComparison.OrdinalIgnoreCase))
            {
stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }
            }
           return await stocks.ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
          return await _context.Stock.Include(c => c.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequest stockDto)
        {
            var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(existingStock == null) {
                return null;
            }
              existingStock.Symbol = stockDto.Symbol;
         existingStock.CompanyName = stockDto.CompanyName;
          existingStock.Purchase = stockDto.Purchase;
           existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;
             existingStock.LastDiv = stockDto.LastDiv;
          await _context.SaveChangesAsync();
          return existingStock;
        }
        public async Task<bool> StockExist(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolasync(string symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
        } 
    }
}