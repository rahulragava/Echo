using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class EditTextPostManager : IEditTextPostManager
    {
        private static EditTextPostManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private EditTextPostManager() { }

        public static EditTextPostManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new EditTextPostManager();
                        }
                    }
                }
                return Instance;
            }
        }
        private readonly ITextPostDbHandler _textPostDbHandler = TextPostDbHandler.GetInstance;

        public async Task EditTextPostAsync(EditTextPostRequest editTextPostRequest,
            EditTextPostUseCaseCallBack editTextPostUseCaseCallBack)
        {
            try
            {
                var textPost = new TextPost()
                {
                    Content = editTextPostRequest.TextPost.Content,
                    CreatedAt = editTextPostRequest.TextPost.CreatedAt,
                    FontStyle = editTextPostRequest.TextPost.FontStyle,
                    Id = editTextPostRequest.TextPost.Id,
                    LastModifiedAt = editTextPostRequest.TextPost.LastModifiedAt,
                    PostedBy = editTextPostRequest.TextPost.PostedBy,
                };
                await _textPostDbHandler.UpdateTextPostAsync(textPost);

                editTextPostUseCaseCallBack?.OnSuccess(new EditTextPostResponse(true));
            }
            catch (Exception e)
            {
                editTextPostUseCaseCallBack?.OnError(e);
            }
        }
    }
}
