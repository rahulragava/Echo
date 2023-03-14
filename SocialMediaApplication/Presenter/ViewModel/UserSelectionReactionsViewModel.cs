using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class UserSelectionReactionsViewModel
    {
        public ObservableCollection<Reaction> Reactions;
        public List<string> UserIds;

        public UserSelectionReactionsViewModel()
        {
            Reactions = new ObservableCollection<Reaction>();
            UserIds = new List<string>();
        }

        public void GetAndSetReactionChosenUsers(ReactionType reactionType)
        {
            UserIds = Reactions.Where(r => r.ReactionType == reactionType).Select(u => u.ReactedBy).ToList();
        }
    }
}
