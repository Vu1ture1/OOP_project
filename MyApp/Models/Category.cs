using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Category
    {
        public Category() { categoty_str = default(string); }

        [Key]
        public int id { get; set; }

        public string categoty_str {  get; set; }
    }
}
