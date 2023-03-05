using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.Constant
{
    public class PostFontStyleEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public PostFontStyle FontStyle { get; set; }
    }
    public enum PostFontStyle
    {
        Simple,
        Casual,
        Fancy
    }
}
