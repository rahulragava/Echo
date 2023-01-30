using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Constant
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
        DontSpecify
    }
}
