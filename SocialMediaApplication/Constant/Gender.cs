using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Constant
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
        DontSpecify
    }
}
