using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Article
    {
        public Article()
        {
            title = string.Empty;

            content = string.Empty;

            date = string.Empty;
        }

        [Key]
        public int id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string date { get; set; }

    }
}
