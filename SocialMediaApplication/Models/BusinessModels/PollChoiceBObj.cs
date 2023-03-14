using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.BusinessModels
{
    public class PollChoiceBObj : PollChoice,INotifyPropertyChanged
    {
        private List<UserPollChoiceSelection> _choiceSelectedUsers;
        public List<UserPollChoiceSelection> ChoiceSelectedUsers
        {
            get => _choiceSelectedUsers;
            set
            {
                if (_choiceSelectedUsers != value)
                {
                    _choiceSelectedUsers = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _choiceSelectionPercent;
        public int ChoiceSelectionPercent
        {
            get => _choiceSelectionPercent;
            set
            {
                if (_choiceSelectionPercent != value)
                {
                    _choiceSelectionPercent = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
