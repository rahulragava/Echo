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
        Happy = 1,
        Sad,
        Mad,
        Confused,
        Heart,
        ThumbsUp,
        ThumbsDown,
        HeartBreak
    }
}
