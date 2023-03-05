using SQLite;

namespace SocialMediaApplication.Models.Constant
{
    public class ReactionTypeEntity
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public ReactionType ReactionType { get; set; }
    }

    public enum ReactionType
    {
        None=1,
        Happy,
        Sad,
        Mad,
        Confused,
        Heart,
        ThumbsUp,
        ThumbsDown,
        HeartBreak
    }
}
