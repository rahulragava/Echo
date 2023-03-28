using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SocialMediaApplication.Domain.UseCase;
using Windows.UI.Core;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Presenter.View;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class SearchViewModel : ObservableObject
    {
        public List<string> UserNames;
        public List<string> UserIds;
        public ObservableCollection<string> UserNameList;
        public ISearchView SearchView { get; set; }

        private string _userId;

        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        public ObservableCollection<string> FilteredUserNames;

        public SearchViewModel()
        {
            UserNames = new List<string>();
            UserNameList = new ObservableCollection<string>();
            FilteredUserNames = new ObservableCollection<string>();
        }
       
        public void GetUserNames()
        {
            var getUserNamesRequestObj = new GetUserNamesRequestObj(new SearchViewModelPresenterCallBack(this));
            var getUserNamesUseCase = new GetUserNamesUseCase(getUserNamesRequestObj);
            getUserNamesUseCase.Execute();
        }

        public void FilterSuggestions(string searchText)
        {
            FilteredUserNames.Clear();
            foreach (var name in UserNames)
            {
                if(name.ToLower().Contains(searchText.ToLower()))
                    FilteredUserNames.Add(name);
            }
        }

        public void PerformSearch(string searchedText)
        {
            string userName;
            if (UserNames.Contains(searchedText))
            {
                userName = searchedText;
            }
            else
            {
                if (FilteredUserNames.Any())
                {
                    userName = FilteredUserNames[0];
                }
                else
                {
                    return;
                }
            }
            var index = UserNames.IndexOf(userName);
            UserId = UserIds[index];
            SearchView.NavigateToProfilePage();
        }

        public void SearchSuccess(GetUserNamesResponseObj getUserNamesResponseObj)
        {
            UserNames = getUserNamesResponseObj.UserNames;
            UserIds = getUserNamesResponseObj.UserIds;
            UserNameList.Clear();
            foreach (var userName in UserNames)
            {
                UserNameList.Add(userName);
            }
        }

        public class SearchViewModelPresenterCallBack : IPresenterCallBack<GetUserNamesResponseObj>
        {
            private readonly SearchViewModel _searchViewModel;

            public SearchViewModelPresenterCallBack(SearchViewModel searchViewModel)
            {
                _searchViewModel = searchViewModel;
            }

            public void OnSuccess(GetUserNamesResponseObj getUserNamesResponseObj)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _searchViewModel.SearchSuccess(getUserNamesResponseObj);
                    }
                );

            }
            public void OnError(Exception ex)
            {
            }
        }
    }

    

}
