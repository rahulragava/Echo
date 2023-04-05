using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Domain.UseCase;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using SocialMediaApplication.Presenter.View.PostView.TextPostView;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class EditTextPostViewModel : ObservableObject
    {
        public TextPostBObj TextPostBObj { get; set; }
        public IEditTextPostUserControl EditTextPostUserControl;
        //public List<PostFontStyle> PostFontStyles;
        
        private string _content;

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        //public EditTextPostViewModel()
        //{
        //    PostFontStyles = Enum.GetValues(typeof(PostFontStyle)).Cast<PostFontStyle>().ToList();
        //}
        //public string OriginalContent { get; set; }
        //public PostFontStyle OriginalFontStyle { get; set; }

        //private PostFontStyle _fontStyle;

        //public PostFontStyle FontStyle
        //{
        //    get => _fontStyle;
        //    set => SetProperty(ref _fontStyle, value);
        //}

        public void EditText()
        {
            TextPostBObj.LastModifiedAt = DateTime.Now;
            TextPostBObj.Content = Content;
            //TextPostBObj.FontStyle = FontStyle;

            var editTextPostRequest = new EditTextPostRequest(TextPostBObj);
            var editTextPostUseCase =
                new EditTextPostUseCase(editTextPostRequest, new EditTextPostViewModelPresenterCallBack(this));
            editTextPostUseCase.Execute();
        }

        public class EditTextPostViewModelPresenterCallBack : IPresenterCallBack<EditTextPostResponse>
        {
            private readonly EditTextPostViewModel _editTextPostViewModel;

            public EditTextPostViewModelPresenterCallBack(EditTextPostViewModel editTextPostViewModel)
            {
                _editTextPostViewModel = editTextPostViewModel;
            }

            public void OnSuccess(EditTextPostResponse editTextPostResponse)
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                       _editTextPostViewModel.EditTextPostUserControl.Close();
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
