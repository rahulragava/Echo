using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.BusinessModels;

namespace SocialMediaApplication.Util
{
    public class SelectTemplate : DataTemplateSelector
    {
        public DataTemplate TextPostDataTemplate { get; set; }
        public DataTemplate PollPostDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object post)
        {
            if (post is TextPostBObj)
                return TextPostDataTemplate;
            if (post is PollPostBObj)
                return PollPostDataTemplate;

            return base.SelectTemplateCore(post);
        }

        protected override DataTemplate SelectTemplateCore(object post, DependencyObject container)
        {
            return SelectTemplateCore(post);
        }

    }

}
