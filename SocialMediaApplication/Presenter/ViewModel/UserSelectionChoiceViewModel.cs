using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.PlatformUI;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class UserSelectionChoiceViewModel : ObservableObject
    {
        private int _choicePercentage = 0;

        public int ChoicePercentage
        {
            get => _choicePercentage;
            set => SetProperty(ref _choicePercentage, value);
        }



    }

}
