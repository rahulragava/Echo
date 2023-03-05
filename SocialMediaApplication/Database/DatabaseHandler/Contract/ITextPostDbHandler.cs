using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface ITextPostDbHandler
    {
        Task InsertTextPostAsync(TextPost textPost);
        Task UpdateTextPostAsync(TextPost textPost);
        Task<TextPost> GetTextPostAsync(string textId);
        Task<IEnumerable<TextPost>> GetAllTextPostAsync();
        Task<IEnumerable<TextPost>> GetUserTextPostsAsync(string userId);
        Task RemoveTextPostAsync(string TextPost);
        Task<IEnumerable<TextPost>> GetSpecificPostAsync(int takeAmount, int skipAmount);
    }
}
