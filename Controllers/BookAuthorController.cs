using Microsoft.AspNetCore.Mvc;
using WebAPITemple.Data;
using WebAPITemple.Models.DTO;
using WebAPITemple.Repositories;

namespace WebAPITemple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly AppDbContext _context;

        public BookAuthorController(IBookAuthorRepository bookAuthorRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        [HttpPost]
        public IActionResult AddBookAuthor([FromBody] AddBookAuthorRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra tồn tại 
            if (!_bookAuthorRepository.BookExists(dto.BookId))
            {
                ModelState.AddModelError(nameof(dto.BookId), "BookID does not exist");
            }

            if (!_bookAuthorRepository.AuthorExists(dto.AuthorId))
            {
                ModelState.AddModelError(nameof(dto.AuthorId), "AuthorID does not exist");
            }
            //
            if (_context.Book_Authors.Any(ba => ba.BookId == dto.BookId && ba.AuthorId == dto.AuthorId))
                return Conflict("Author already assigned to this Book");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookAuthorRepository.AddBookAuthor(dto);
            return Ok("Book_Author added successfully");
        }
    }
}
