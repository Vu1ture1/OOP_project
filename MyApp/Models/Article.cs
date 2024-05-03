using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Article
    {
        public Article()
        {
            title = string.Empty;

            user_id = default(int);

            content = string.Empty;

            date = string.Empty;

            path_to_corer = string.Empty;

            status = false;

            categories = new List<Category>();
        }

        [Key]
        public int id { get; set; }
        public int user_id {  get; set; }
        public string title { get; set; }

        public bool status {  get; set; }
        public List<Category> categories { get; set; }

        public string path_to_corer {  get; set; }

        public string content { get; set; }

        public string date { get; set; }

    }
}
