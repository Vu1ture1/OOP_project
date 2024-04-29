using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class UserInfo
    {
        public UserInfo() { email = string.Empty; firstname = string.Empty; lastname = string.Empty; }

        [Key]
        public int user_info_id {  get; set; }
        public string email { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

    }
}
