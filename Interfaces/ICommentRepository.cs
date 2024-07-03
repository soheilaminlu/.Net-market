using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Dto;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
       Task<List<Comment>> GetAllCommentAsync();
       Task<Comment?> GetByIdAsync(int id);
        
       Task<Comment> CreateAsync(Comment commentModel);

       Task<Comment> UpdateAsync(int commentId  , UpdateRequestComment commentDto);

       Task<Comment> DeleteAsync(int commentId);
 
    }
}