using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Comment
    {
        public Comment()
        {
            creator = null;

            context = string.Empty;

            created = string.Empty;
        }
        
        [Key]

        public int id { get; set; }

        public User creator { get; set; }

        public string context {  get; set; }

        public string created { get; set; }
    }
}
