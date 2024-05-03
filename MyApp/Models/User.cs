using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class User
    {
        
        public User() { username = string.Empty; user_role = string.Empty; password = string.Empty; user_info = new UserInfo(); path_to_icon = string.Empty; serialize_article_ids = string.Empty; }

        [Key]
        public int id { get; set; }

        //[Required(ErrorMessage = "Введите имя")]
        public string username { get; set; }

        public string password { get; set; }

        public string serialize_article_ids { get; set; }

        public string user_role { get; set; }

        public string path_to_icon {  get; set; }
        public UserInfo user_info { get; set; }
    }
}
