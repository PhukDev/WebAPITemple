using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;
using WebAPITemple.Repositories;

namespace WebAPITemple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;  

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("get-all-books")]
        public async Task<IActionResult> GetAllAsync()  // Async
        {
            var allBooks = await _bookRepository.GetAllBooksAsync();  // Await
            return Ok(allBooks);
        }

        [HttpGet("get-book-by-id/{id}")]
        public async Task<IActionResult> GetBookByIdAsync(int id)  // Async
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return NotFound();  // Xử lý null
            return Ok(book);
        }

        [HttpPost("add-book")]
        public async Task<IActionResult> AddBookAsync([FromBody] addBookRequestDTO addBookRequestDTO)  // Async
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var bookAdded = await _bookRepository.AddBookAsync(addBookRequestDTO);
            return Ok(bookAdded);
        }

        [HttpPut("update-book-by-id/{id}")]
        public async Task<IActionResult> UpdateBookByIdAsync(int id, [FromBody] addBookRequestDTO bookDTO)  // Async
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedBook = await _bookRepository.UpdateBookByIdAsync(id, bookDTO);
            if (updatedBook == null) return NotFound();
            return Ok(updatedBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        public async Task<IActionResult> DeleteBookByIdAsync(int id)  // Async
        {
            var deletedBook = await _bookRepository.DeleteBookByIdAsync(id);
            if (deletedBook == null) return NotFound();
            return Ok(deletedBook);
        }
    }
}
