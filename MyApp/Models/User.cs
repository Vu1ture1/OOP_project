using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class User
    {
        
        public User() { username = string.Empty; user_role = string.Empty; password = string.Empty; user_info = new UserInfo(); path_to_icon = string.Empty; channel_articles = new List<Article>(); my_subscribes = new List<User>(); subscribers_num = 0; Liked_articles = new List<int>(); channel_description = string.Empty; }

        [Key]
        public int id { get; set; }

        //[Required(ErrorMessage = "Введите имя")]
        public string username { get; set; }

        public string channelname { get; set; }

        public string password { get; set; }

        public string salt {  get; set; }

        public List<Article>  channel_articles { get; set; }

        public string user_role { get; set; }

        public List<int> Liked_articles { get; set; }
        
        public string path_to_icon {  get; set; }

        public string path_to_channel_icon { get; set; }

        public string path_to_channel_image { get; set; }

        public string channel_description { get; set; }
        public UserInfo user_info { get; set; }

        public List<User> my_subscribes { get; set; }

        public int subscribers_num { get; set; }
    }
}
