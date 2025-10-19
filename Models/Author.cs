using System.Collections.Generic;
namespace Course4.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}