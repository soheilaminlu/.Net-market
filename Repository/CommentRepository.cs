
using api.Data;
using api.Dto;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Comment>> GetAllCommentAsync()
        {
         return await _context.Comments.Include(c => c.User).ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(int id)
        {
          return await _context.Comments.FindAsync(id);
        }
       public async Task<Comment> CreateAsync(Comment commentModel)
       {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
       }
       public async Task<Comment> UpdateAsync(int commentId  , UpdateRequestComment commentDto)
       {
        var existingComment = await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id  == commentId);
        if(existingComment == null) {
            return null;
        }
        existingComment.Content = commentDto.Content;
        existingComment.Title = commentDto.Title;
        await _context.SaveChangesAsync();
        return existingComment;
       }
       public async Task<Comment> DeleteAsync(int commentId)
       {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
         if(comment == null) {
            return null;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return comment;

       }
    }
}