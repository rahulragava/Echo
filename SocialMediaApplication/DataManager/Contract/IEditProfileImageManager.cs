using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IEditProfileImageManager
    {
        Task EditProfileImageAsync(EditProfileImageRequest request, EditProfileImageUseCaseCallBack editProfileImageUseCaseCallBack);
        Task EditHomeImageAsync(EditHomeImageRequest request, EditHomeImageUseCaseCallBack editHomeImageUseCaseCallBack);
    }

}
