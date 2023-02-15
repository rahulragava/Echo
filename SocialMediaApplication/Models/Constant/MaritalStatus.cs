using SQLite;

namespace SocialMediaApplication.Models.Constant
{
    public class MaritalStatusEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
    }
    public enum MaritalStatus
    {
        Married,
        Unmarried,
        DoNotSpecify
    }
}
