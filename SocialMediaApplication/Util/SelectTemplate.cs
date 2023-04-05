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
            switch (post)
            {
                case TextPostBObj _:
                    return TextPostDataTemplate;
                case PollPostBObj _:
                    return PollPostDataTemplate;
                default:
                    return base.SelectTemplateCore(post);
            }
        }

        protected override DataTemplate SelectTemplateCore(object post, DependencyObject container)
        {
            return SelectTemplateCore(post);
        }

    }

}
