using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.EntityModels
{
    public class UserPollChoiceSelection
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ChoiceId { get; set; }
        public string SelectedBy { get; set; }

        public UserPollChoiceSelection() { }

        public UserPollChoiceSelection(string choiceId, string selectedBy)
        {
            Id = Guid.NewGuid().ToString();
            ChoiceId = choiceId;
            SelectedBy = selectedBy;
        }
    }
}
