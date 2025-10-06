using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebAPITemple.CustomActionFilter;
using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;
using WebAPITemple.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPITemple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly AppDbContext _context;

        public BooksController(IBookRepository bookRepository, AppDbContext context)
        {
            _bookRepository = bookRepository;
            _context = context  ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("get-all-books")]
        [Authorize(Roles = "Read")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)  // Async
        {
            var allBooks = await _bookRepository.GetAllBooksAsync(filterOn,filterQuery, sortBy, isAscending, pageNumber, pageSize);  // Await
            return Ok(allBooks);
        }

        [HttpGet("get-book-by-id/{id}")]
        [Authorize(Roles = "Read")]
        public async Task<IActionResult> GetBookByIdAsync(int id)  // Async
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return NotFound();  // Xử lý null
            return Ok(book);
        }

        [HttpPost("add-book")]
        [Authorize(Roles ="Write")]
        [ValidateModel]  // Custom Action Filter
        //  [Authorize(Roles = "Write")]
        public async Task<IActionResult> AddBookAsync([FromBody] addBookRequestDTO addBookRequestDTO)  // Async
        {
            if (!ValidateAddBook(addBookRequestDTO)) return BadRequest(ModelState);
            var bookAdded = await _bookRepository.AddBookAsync(addBookRequestDTO);
            return Ok(bookAdded);
        }

        [HttpPut("update-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public async Task<IActionResult> UpdateBookByIdAsync(int id, [FromBody] addBookRequestDTO bookDTO)  // Async
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedBook = await _bookRepository.UpdateBookByIdAsync(id, bookDTO);
            if (updatedBook == null) return NotFound();
            return Ok(updatedBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public async Task<IActionResult> DeleteBookByIdAsync(int id)  // Async
        {
            var deletedBook = await _bookRepository.DeleteBookByIdAsync(id);
            if (deletedBook == null) return NotFound();
            return Ok(deletedBook);
        }

        private bool ValidateAddBook(addBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), $"Please add book data"); 
                return false;
            }
            // kiem tra Description NotNull 
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description), $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }
            // kiem tra rating (0,5) 
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate), $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            if (!_context.Publishers.Any(p => p.Id == addBookRequestDTO.PublisherId))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.PublisherId), "PublisherID does not exist");
                return false;
            }
            return true;
        }


    }
}
