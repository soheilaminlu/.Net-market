using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetStockByIdAsync(int id);
          
        Task<Stock> CreateStockAsync(Stock stockModel);

        Task<Stock> GetBySymbolasync(string symbol);

        Task<Stock?> UpdateStockAsync(int id , UpdateStockRequest stockDto);

        Task<Stock?> DeleteStockAsync(int id);

        Task<bool> StockExist(int id);
        
    }
}