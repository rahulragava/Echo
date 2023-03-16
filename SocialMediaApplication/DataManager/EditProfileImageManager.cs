using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager
{
    public sealed class EditProfileImageManager : IEditProfileImageManager
    {
        private static EditProfileImageManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private EditProfileImageManager() { }

        public static EditProfileImageManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new EditProfileImageManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        
        public async Task EditProfileImage(EditProfileImageRequest request, EditProfileImageUseCaseCallBack editProfileImageUseCaseCallBack)
        {
            try
            {
                var user = await _userDbHandler.GetUserAsync(request.UserId);
                    user.ProfileIcon = request.ImagePath;

                await _userDbHandler.UpdateUserAsync(user);
                editProfileImageUseCaseCallBack.OnSuccess(new EditProfileImageResponse(true));
            }
            catch (Exception e)
            {
                editProfileImageUseCaseCallBack.OnError(e);
            }
        }

        public async Task EditHomeImage(EditHomeImageRequest request, EditHomeImageUseCaseCallBack editHomeImageUseCaseCallBack)
        {
            try
            {
                var user = await _userDbHandler.GetUserAsync(request.UserId);
                user.HomePageIcon = request.ImagePath;

                await _userDbHandler.UpdateUserAsync(user);
                editHomeImageUseCaseCallBack.OnSuccess(new EditHomeImageResponse(true));
            }
            catch (Exception e)
            {
                editHomeImageUseCaseCallBack.OnError(e);
            }
        }


    }
}
