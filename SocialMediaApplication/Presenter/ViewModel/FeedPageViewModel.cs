using SocialMediaApplication.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using SocialMediaApplication.Domain.UseCase;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;
using static SocialMediaApplication.Presenter.ViewModel.GetUsersDetailViewModel;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.View.PostView.TextPostView;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class FeedPageViewModel : ObservableObject
    {
        public ObservableCollection<TextPostBObj> TextPosts;
        public ObservableCollection<PollPostBObj> PollPosts;
        public ObservableCollection<PostBObj> PostBObjList;
        public ObservableCollection<Reaction> Reactions;
        public ObservableCollection<Reaction> CommentReactions;
        public bool Success = true;
        public User User;
        public IMiniTextPostCreationView MiniTextPostCreationView;

        public FeedPageViewModel()
        {
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
            PostBObjList = new ObservableCollection<PostBObj>();
            Reactions = new ObservableCollection<Reaction>();
            CommentReactions = new ObservableCollection<Reaction>();
            PostFontStyles = Enum.GetValues(typeof(PostFontStyle)).Cast<PostFontStyle>().ToList();
        }
        private double _scrollPosition;

        public double ScrollPosition
        {
            get => _scrollPosition;
            set => SetProperty(ref _scrollPosition,value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private BitmapImage _profileImage;

        public BitmapImage ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        private string _userThought;

        public string UserThought
        {
            get => _userThought;
            set => SetProperty(ref _userThought, value);
        }
     
        public async void SetProfileIcon(string imagePath)
        {
            var imageConversion = new StringToImageUtil();
            var profileIcon = await imageConversion.GetImageFromStringAsync(imagePath);
            ProfileImage = profileIcon;
        }
        public void SetReactions(List<Reaction> reactions)
        {
            Reactions.Clear();
            foreach (var reaction in reactions)
            {
                Reactions.Add(reaction);
            }
        }

        public void SetCommentReactions(List<Reaction> reactions)
        {
            CommentReactions.Clear();
            foreach (var reaction in reactions)
            {
                CommentReactions.Add(reaction);
            }
        }
        public List<PostFontStyle> PostFontStyles;

        private PostFontStyle _fontStyle = PostFontStyle.Simple;

        public PostFontStyle FontStyle
        {
            get => _fontStyle;

            set => SetProperty(ref _fontStyle, value);
        }

        private Reaction _reaction;

        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }

        public void ChangeInReactions(List<Reaction> reactions)
        {
            var flag = false;
            foreach (var reaction in reactions)
            {
                if (reaction.ReactedBy == Reaction.ReactedBy)
                {
                    Reactions.Remove(reaction);
                    Reactions.Add(Reaction);
                    flag = true;
                }
            }
            if (!flag)
            {
                Reactions.Add(Reaction);
            }
            flag = false;
        }

        public Reaction CommentReaction { get; set; }


        public void ChangeInCommentReactions(List<Reaction> reactions)
        {
            var flag = false;
            foreach (var reaction in reactions)
            {
                if (reaction.ReactedBy == CommentReaction.ReactedBy)
                {
                    CommentReactions.Remove(reaction);
                    CommentReactions.Add(CommentReaction);
                    flag = true;
                }
            }
            if (!flag)
            {
                CommentReactions.Add(CommentReaction);
            }
            flag = false;
        }

       

        public int PollAmountToBeFetched = 5;
        public int PollAmountToBeSkipped = 0;
        public int TextAmountToBeFetched = 5;
        public int TextAmountToBeSkipped = 0;

        public void GetFeeds()
        {
            Success = false;
            var fetchFeedRequest = new FetchFeedRequest(PollAmountToBeFetched, PollAmountToBeSkipped, TextAmountToBeFetched, TextAmountToBeSkipped);
            var fetchFeedUseCase = new FetchFeedUseCase(fetchFeedRequest, new FeedPageViewModelPresenterCallBack(this));
            fetchFeedUseCase.Execute();
        }

        public void GetUser()
        {
            var getUserRequest = new GetUserRequestObj(new List<string>(){AppSettings.UserId});
            var getUserUseCase = new GetUserUseCase(getUserRequest, new GetUserDetailViewModelPresenterCallBack(this));
            getUserUseCase.Execute();
        }

        public TextPost TextPost;
        public void CreateTextPost(TextPost textPost)
        {
            TextPost = textPost;
            var textPostCreationRequest =
                new TextPostCreationRequest(textPost);
            var textPostCreationUseCase = new TextPostCreationUseCase(textPostCreationRequest, new TextPostCreationPresenterCallBack(this));
            textPostCreationUseCase.Execute();
        }

        private TextPostBObj _textPostBObj;

        public TextPostBObj TextPostBObj
        {
            get => _textPostBObj;
            set => SetProperty(ref _textPostBObj,value);
        }
        public class GetUserDetailViewModelPresenterCallBack : IPresenterCallBack<GetUserResponseObj>
        {
            private readonly FeedPageViewModel _feedPageViewModel;

            public GetUserDetailViewModelPresenterCallBack(FeedPageViewModel feedPageViewModel)
            {
                _feedPageViewModel  = feedPageViewModel;
            }

            public void OnSuccess(GetUserResponseObj getUserResponseObj)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _feedPageViewModel.User = getUserResponseObj.Users[0];

                        if (!string.IsNullOrEmpty(getUserResponseObj.Users[0].FirstName))
                        {
                            var userThought = _feedPageViewModel.MiniTextPostCreationView.GetUserDetailSuccess(getUserResponseObj.Users[0].FirstName);
                            _feedPageViewModel.UserThought = userThought;
                        }
                        else
                        {
                            var userThought = _feedPageViewModel.MiniTextPostCreationView.GetUserDetailSuccess(getUserResponseObj.Users[0].UserName);
                            _feedPageViewModel.UserThought = userThought;
                        }
                        _feedPageViewModel.SetProfileIcon(getUserResponseObj.Users[0].ProfileIcon);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }

        public class FeedPageViewModelPresenterCallBack : IPresenterCallBack<FetchFeedResponse>
        {
            private readonly FeedPageViewModel _feedPageViewModel;
            public FeedPageViewModelPresenterCallBack(FeedPageViewModel feedPageViewModel)
            {
                _feedPageViewModel = feedPageViewModel;
            }

            public void OnSuccess(FetchFeedResponse fetchFeedResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                     () =>
                    {
                        if (fetchFeedResponse.TextPosts.Count == 0 && fetchFeedResponse.PollPosts.Count == 0)
                        {
                            //_feedPageViewModel.IsLoading = false;
                            //_feedPageViewModel.Success = true;
                            return;
                        }
                        foreach (var postBObj in fetchFeedResponse.TextPosts.OrderByDescending(p => p.CreatedAt))
                        {
                            _feedPageViewModel.PostBObjList.Add(postBObj);
                        }

                        foreach (var postBObj in fetchFeedResponse.PollPosts.OrderByDescending(p => p.CreatedAt))
                        {
                            _feedPageViewModel.PostBObjList.Add(postBObj);
                        }

                        //await Task.Delay(500);
                        //_feedPageViewModel.IsLoading = false;
                        _feedPageViewModel.PollAmountToBeSkipped += 5;
                        _feedPageViewModel.TextAmountToBeSkipped += 5;
                        _feedPageViewModel.Success = true;
                    }
                );
            }

            public void OnError(Exception ex)
            {
                
            }
        }

        public class TextPostCreationPresenterCallBack : IPresenterCallBack<TextPostCreationResponse>
        {
            private readonly FeedPageViewModel _feedPageViewModel;
            public TextPostCreationPresenterCallBack(FeedPageViewModel feedPageViewModel)
            {
                _feedPageViewModel = feedPageViewModel;
            }


            public void OnSuccess(TextPostCreationResponse textPostCreationResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //Notify
                        _feedPageViewModel.MiniTextPostCreationView.PostedSuccess();
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
