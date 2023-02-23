using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface ICommentDbHandler
    {
        Task InsertCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task<Comment> GetCommentAsync(string commentId);
        Task<IEnumerable<Comment>> GetAllCommentAsync();
        Task RemoveCommentAsync(string commentId);
    }
}
