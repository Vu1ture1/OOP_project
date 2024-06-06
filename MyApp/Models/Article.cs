using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyApp.Models
{
    public class Article
    {
        public Article()
        {
            title = string.Empty;

            user = new User();

            content = string.Empty;

            date = string.Empty;

            path_to_corer = string.Empty;

            status = false;

            likes = 0;

            comments = new List<Comment>();

            category = new Category();
        }

        [Key]
        public int id { get; set; }
        public User user {  get; set; }
        public string title { get; set; }

        public List<Comment> comments { get; set; }
        public int likes { get; set; }
        public bool status {  get; set; }
        public Category category { get; set; }

        public string path_to_corer {  get; set; }

        public string content { get; set; }

        public string date { get; set; }

    }
}
