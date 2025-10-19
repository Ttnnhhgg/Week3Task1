using BookAPI.Data;
using Course4.Models;
using Microsoft.AspNetCore.Mvc;
namespace Course4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {       
        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            try
            {
                // Возвращение всех авторов с их книгами
                var authors = InMemoryData.Authors.Select(a => new Author
                {
                    Id = a.Id,
                    Name = a.Name,
                    DateOfBirth = a.DateOfBirth,
                    Books = InMemoryData.Books.Where(b => b.AuthorId == a.Id).ToList()
                }).ToList();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }        
        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id)
        {
            try
            {
                var author = InMemoryData.Authors.FirstOrDefault(a => a.Id == id);
                if (author == null)
                {
                    return NotFound($"Автор с ID {id} не найден");
                }
                // Создание автора с книгами
                var authorWithBooks = new Author
                {
                    Id = author.Id,
                    Name = author.Name,
                    DateOfBirth = author.DateOfBirth,
                    Books = InMemoryData.Books.Where(b => b.AuthorId == author.Id).ToList()
                };
                return Ok(authorWithBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }       
        [HttpPost]
        public ActionResult<Author> CreateAuthor([FromBody] Author author)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(author.Name))
                {
                    return BadRequest("Имя автора обязательно для заполнения");
                }
                if (author.DateOfBirth > DateTime.Now)
                {
                    return BadRequest("Дата рождения не может быть в будущем");
                }
                // Создание нового автора
                var newAuthor = new Author
                {
                    Id = InMemoryData.GetNextAuthorId(),
                    Name = author.Name.Trim(),
                    DateOfBirth = author.DateOfBirth
                };
                InMemoryData.Authors.Add(newAuthor);                
                return CreatedAtAction(nameof(GetAuthor), new { id = newAuthor.Id }, newAuthor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании автора: {ex.Message}");
            }
        }       
        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Author author)
        {
            try
            {
                if (id != author.Id)
                {
                    return BadRequest("ID в URL не совпадает с ID в теле запроса");
                }
                var existingAuthor = InMemoryData.Authors.FirstOrDefault(a => a.Id == id);
                if (existingAuthor == null)
                {
                    return NotFound($"Автор с ID {id} не найден");
                }
                // Валидация
                if (string.IsNullOrWhiteSpace(author.Name))
                {
                    return BadRequest("Имя автора обязательно для заполнения");
                }
                if (author.DateOfBirth > DateTime.Now)
                {
                    return BadRequest("Дата рождения не может быть в будущем");
                }               
                existingAuthor.Name = author.Name.Trim();
                existingAuthor.DateOfBirth = author.DateOfBirth;
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении автора: {ex.Message}");
            }
        }       
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            try
            {
                var author = InMemoryData.Authors.FirstOrDefault(a => a.Id == id);
                if (author == null)
                {
                    return NotFound($"Автор с ID {id} не найден");
                }               
                var authorBooks = InMemoryData.Books.Where(b => b.AuthorId == id);
                if (authorBooks.Any())
                {
                    return BadRequest("Нельзя удалить автора, у которого есть книги. Сначала удалите все книги автора.");
                }
                InMemoryData.Authors.Remove(author);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении автора: {ex.Message}");
            }
        }
    }
}
