using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface ILabelDbHandler
    {
        Task InsertLabelAsync(Label label);
        Task UpdateLabelAsync(Label label);
        Task<Label> GetLabelAsync(string labelId);
        Task<IEnumerable<Label>> GetAllLabelAsync();
        Task RemoveLabelAsync(string labelId);
    }
}
