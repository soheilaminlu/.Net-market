
using api.Dto;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Content = commentModel.Content,
                Title = commentModel.Title,
                StockId = commentModel.StockId,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.User.UserName
            };
        }

         public static Comment ToCommentFromCreate(this CreateCommentDto commentDto , int stockId)
        {
            return new Comment
            {
                Content = commentDto.Content ?? string.Empty,
                Title = commentDto.Title ?? string.Empty,
                StockId = stockId
            };
        }

    }
}