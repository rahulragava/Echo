using SQLite;

namespace SocialMediaApplication.Models.Constant
{
    public class GenderEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Gender Gender { get; set; }
    }
    public enum Gender
    {
        Male,
        Female,
        DoNotSpecify
    }
}
