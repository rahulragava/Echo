using SocialMediaApplication.Domain.UseCase;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IAddCommentManager
    {
        Task InsertCommentAsync(InsertCommentRequest insertCommentRequest, InsertCommentUseCaseCallBack insertCommentUseCaseCallBack);
    }
}
