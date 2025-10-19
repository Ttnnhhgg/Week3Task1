using Course4.Models;
namespace BookAPI.Data
{
    public static class InMemoryData
    {
        public static List<Author> Authors = new List<Author>
        {
            new Author { Id = 1, Name = "Лев Толстой", DateOfBirth = new DateTime(1828, 9, 9) },
            new Author { Id = 2, Name = "Фёдор Достоевский", DateOfBirth = new DateTime(1821, 11, 11) },
            new Author { Id = 3, Name = "Антон Чехов", DateOfBirth = new DateTime(1860, 1, 29) }
        };
        public static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "Война и мир", PublishedYear = 1869, AuthorId = 1 },
            new Book { Id = 2, Title = "Анна Каренина", PublishedYear = 1877, AuthorId = 1 },
            new Book { Id = 3, Title = "Преступление и наказание", PublishedYear = 1866, AuthorId = 2 },
            new Book { Id = 4, Title = "Вишнёвый сад", PublishedYear = 1904, AuthorId = 3 }
        };
        private static int _nextAuthorId = 4;
        private static int _nextBookId = 5;
        public static int GetNextAuthorId() => _nextAuthorId++;
        public static int GetNextBookId() => _nextBookId++;
    }
}
