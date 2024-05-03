using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class ArticleRequest
    {
        public ArticleRequest() { article = null; }
        
        [Key]
        public int id { get; set; }

        public Article article { get; set; }
    }
}
