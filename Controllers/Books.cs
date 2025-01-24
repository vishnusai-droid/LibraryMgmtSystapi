using Microsoft.AspNetCore.Mvc;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repositories;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookRepository _repository;

        public BooksController(BookRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (string.IsNullOrEmpty(book.ISBN))
                return BadRequest("ISBN is required.");

            if (!_repository.AddBook(book))
                return Conflict($"A book with ISBN {book.ISBN} already exists.");

            return CreatedAtAction(nameof(GetBookByISBN),
                new { isbn = book.ISBN }, book);
        }

        [HttpPut("{isbn}")]
        public IActionResult UpdateBook(string isbn, [FromBody] Book updatedBook)
        {
            if (isbn != updatedBook.ISBN)
                return BadRequest("ISBN in URL must match book's ISBN.");

            if (!_repository.UpdateBook(isbn, updatedBook))
                return NotFound($"Book with ISBN {isbn} not found.");

            return Ok(updatedBook);
        }

        [HttpDelete("{isbn}")]
        public IActionResult RemoveBook(string isbn)
        {
            if (!_repository.RemoveBook(isbn))
                return NotFound($"Book with ISBN {isbn} not found.");

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllBooks() =>
            Ok(_repository.GetAllBooks());

        [HttpGet("{title}")]
        public IActionResult GetBooksByTitle(string title) =>
            Ok(_repository.SearchBooksByTitle(title));

        [HttpGet("isbn/{isbn}")]
        public IActionResult GetBookByISBN(string isbn)
        {
            var book = _repository.GetBookByISBN(isbn);
            return book != null
                ? Ok(book)
                : NotFound($"Book with ISBN {isbn} not found.");
        }
    }
}