using BookAPI.Data;
using Course4.Models;
using Microsoft.AspNetCore.Mvc;
namespace Course4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {        
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            try
            {
                var books = InMemoryData.Books.Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    PublishedYear = b.PublishedYear,
                    AuthorId = b.AuthorId,
                    Author = InMemoryData.Authors.FirstOrDefault(a => a.Id == b.AuthorId)
                }).ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }        
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            try
            {
                var book = InMemoryData.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return NotFound($"Книга с ID {id} не найдена");
                }
                var bookWithAuthor = new Book
                {
                    Id = book.Id,
                    Title = book.Title,
                    PublishedYear = book.PublishedYear,
                    AuthorId = book.AuthorId,
                    Author = InMemoryData.Authors.FirstOrDefault(a => a.Id == book.AuthorId)
                };
                return Ok(bookWithAuthor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }        
        [HttpPost]
        public ActionResult<Book> CreateBook([FromBody] Book book)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(book.Title))
                {
                    return BadRequest("Название книги обязательно для заполнения");
                }
                if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
                {
                    return BadRequest($"Год публикации должен быть между 1000 и {DateTime.Now.Year}");
                }               
                var author = InMemoryData.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
                if (author == null)
                {
                    return BadRequest("Автор с указанным ID не существует");
                }
                // Создание новой книги
                var newBook = new Book
                {
                    Id = InMemoryData.GetNextBookId(),
                    Title = book.Title.Trim(),
                    PublishedYear = book.PublishedYear,
                    AuthorId = book.AuthorId
                };
                InMemoryData.Books.Add(newBook);
                return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании книги: {ex.Message}");
            }
        }        
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            try
            {
                if (id != book.Id)
                {
                    return BadRequest("ID в URL не совпадает с ID в теле запроса");
                }
                var existingBook = InMemoryData.Books.FirstOrDefault(b => b.Id == id);
                if (existingBook == null)
                {
                    return NotFound($"Книга с ID {id} не найдена");
                }
                // Валидация
                if (string.IsNullOrWhiteSpace(book.Title))
                {
                    return BadRequest("Название книги обязательно для заполнения");
                }
                if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
                {
                    return BadRequest($"Год публикации должен быть между 1000 и {DateTime.Now.Year}");
                }                
                var author = InMemoryData.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
                if (author == null)
                {
                    return BadRequest("Автор с указанным ID не существует");
                }
                // Обновление данных
                existingBook.Title = book.Title.Trim();
                existingBook.PublishedYear = book.PublishedYear;
                existingBook.AuthorId = book.AuthorId;
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении книги: {ex.Message}");
            }
        }       
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var book = InMemoryData.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return NotFound($"Книга с ID {id} не найдена");
                }
                InMemoryData.Books.Remove(book);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении книги: {ex.Message}");
            }
        }      
        [HttpGet("byauthor/{authorId}")]
        public ActionResult<IEnumerable<Book>> GetBooksByAuthor(int authorId)
        {
            try
            {
                var author = InMemoryData.Authors.FirstOrDefault(a => a.Id == authorId);
                if (author == null)
                {
                    return NotFound($"Автор с ID {authorId} не найден");
                }
                var books = InMemoryData.Books
                    .Where(b => b.AuthorId == authorId)
                    .Select(b => new Book
                    {
                        Id = b.Id,
                        Title = b.Title,
                        PublishedYear = b.PublishedYear,
                        AuthorId = b.AuthorId,
                        Author = author
                    })
                    .ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}
