using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;
using Windows.UI.Xaml.Media.Imaging;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class PostCreationPageViewModel : ObservableObject
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title,value);
        }

        private string _content;

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public List<PostFontStyle> PostFontStyles;

        public PostCreationPageViewModel()
        {
            //PostFontStyles.Add();
            PostFontStyles = Enum.GetValues(typeof(PostFontStyle)).Cast<PostFontStyle>().ToList();
        }
       

        private string _pollChoice1;

        public string PollChoice1
        {
            get => _pollChoice1;
            set => SetProperty(ref _pollChoice1, value);
        }

        private string _pollChoice2;

        public string PollChoice2
        {
            get => _pollChoice2;
            set => SetProperty(ref _pollChoice2, value);
        }

        private string _pollChoice3;

        public string PollChoice3
        {
            get => _pollChoice3;
            set => SetProperty(ref _pollChoice3, value);
        }
        private string _pollChoice4;

        public string PollChoice4
        {
            get => _pollChoice4;
            set => SetProperty(ref _pollChoice4, value);
        }

        private PostFontStyle _fontStyle = PostFontStyle.Simple;

        public PostFontStyle FontStyle
        {
            get => _fontStyle;
            
            set => SetProperty(ref _fontStyle, value);
        }

        public void DiscardButtonClicked()
        {
            Title = string.Empty;
            Content = string.Empty;
            PollChoice1 = string.Empty;
            PollChoice2 = string.Empty;
            PollChoice3 = string.Empty;
            PollChoice4 = string.Empty;
            FontStyle = PostFontStyle.Simple;
        }

        private BitmapImage _profileIcon;

        public BitmapImage ProfileIcon
        {
            get => _profileIcon;
            set => SetProperty(ref _profileIcon, value);
        }

        public void CreatePollPost(int pollChoiceCount)
        {
            var pollPost = new PollPost()
            {
                Title = Title,
                Question = Content,
                CreatedAt = DateTime.Now,
                FontStyle = FontStyle,
                LastModifiedAt = DateTime.Now,
                PostedBy = AppSettings.LocalSettings.Values["user"].ToString()
            };

            var choices = new List<PollChoice>();

            var pollChoice1 = new PollChoice()
            {
                Choice = PollChoice1,
                PostId = pollPost.Id,
                PostFontStyle = FontStyle
            };
            var pollChoice2 = new PollChoice()
            {
                Choice = PollChoice2,
                PostId = pollPost.Id,
                PostFontStyle = FontStyle

            };

            choices.Add(pollChoice1);
            choices.Add(pollChoice2);
            PollChoice pollChoice3;
            PollChoice pollChoice4;

            switch (pollChoiceCount)
            {
                case 2:
                    break;
                case 3:
                    pollChoice3 = new PollChoice()
                    {
                        Choice = PollChoice3,
                        PostFontStyle = FontStyle,
                        PostId = pollPost.Id,
                    };
                    choices.Add(pollChoice3);
                    break;
                case 4:
                    pollChoice3 = new PollChoice()
                    {
                        Choice = PollChoice3,
                        PostId = pollPost.Id,
                        PostFontStyle = FontStyle

                    };
                    pollChoice4 = new PollChoice()
                    {
                        Choice = PollChoice4,
                        PostId = pollPost.Id,
                        PostFontStyle = FontStyle

                    };
                    choices.Add(pollChoice4);
                    choices.Add(pollChoice3);
                    break;
            }

            var pollPostCreationRequest =
                new PollPostCreationRequest(pollPost, choices, new PollPostCreationPresenterCallBack(this));
            var pollPostCreationUseCase = new PollPostCreationUseCase(pollPostCreationRequest);
            pollPostCreationUseCase.Execute();
        }

        public void CreateTextPost()
        {
            var textPost = new TextPost()
            {
                Content = Content,
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                FontStyle = FontStyle,
                PostedBy = AppSettings.LocalSettings.Values["user"].ToString(),
                Title = Title
            };

            var textPostCreationRequest =
                new TextPostCreationRequest(textPost , new TextPostCreationPresenterCallBack(this));
            var textPostCreationUseCase = new TextPostCreationUseCase(textPostCreationRequest);
            textPostCreationUseCase.Execute();
        }

        public void GetUser()
        {
            var getUserRequest = new GetUserRequestObj(new List<string>() { AppSettings.UserId },
                new GetUserDetailViewModelPresenterCallBack(this));
            var getUserUseCase = new GetUserUseCase(getUserRequest);
            getUserUseCase.Execute();
        }

        public async Task SetProfileIconAsync(string imagePath)
        {
            var imageConversion = new StringToImageUtil();
            var profileIcon = await imageConversion.GetImageFromStringAsync(imagePath);
            ProfileIcon = profileIcon;
        }

        public void SuccessfullyPostCreated()
        {
            Title = string.Empty;
            Content = string.Empty;
            PollChoice1 = string.Empty;
            PollChoice2 = string.Empty;
            PollChoice3 = string.Empty;
            PollChoice4 = string.Empty;
            FontStyle = PostFontStyle.Simple;
        }

        public class PollPostCreationPresenterCallBack : IPresenterCallBack<PollPostCreationResponse>
        {
            private readonly PostCreationPageViewModel _postCreationPageViewModel;

            public PollPostCreationPresenterCallBack(PostCreationPageViewModel postCreationPageViewModel)
            {
                _postCreationPageViewModel = postCreationPageViewModel;
            }

            public void OnSuccess(PollPostCreationResponse logInResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postCreationPageViewModel.SuccessfullyPostCreated();
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class TextPostCreationPresenterCallBack : IPresenterCallBack<TextPostCreationResponse>
        {
            private readonly PostCreationPageViewModel _postCreationPageViewModel;

            public TextPostCreationPresenterCallBack(PostCreationPageViewModel postCreationPageViewModel)
            {
                _postCreationPageViewModel = postCreationPageViewModel;
            }


            public void OnSuccess(TextPostCreationResponse logInResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postCreationPageViewModel.SuccessfullyPostCreated();
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class GetUserDetailViewModelPresenterCallBack : IPresenterCallBack<GetUserResponseObj>
        {
            private readonly PostCreationPageViewModel _postCreationPageViewModel;

            public GetUserDetailViewModelPresenterCallBack(PostCreationPageViewModel postCreationPageViewModel)
            {
                _postCreationPageViewModel = postCreationPageViewModel;
            }

            public void OnSuccess(GetUserResponseObj getUserResponseObj)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postCreationPageViewModel.UserName = getUserResponseObj.Users[0].UserName;
                        _postCreationPageViewModel?.SetProfileIconAsync(getUserResponseObj.Users[0].ProfileIcon);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }
    }
}
