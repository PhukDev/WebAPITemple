using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;
using WebAPITemple.Repositories;

namespace WebAPITemple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {

        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;
        public AuthorsController(AppDbContext dbContext, IAuthorRepository
authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthor()
        {
            var allAuthors = _authorRepository.GetAllAuthors();
            return Ok(allAuthors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var authorWithId = _authorRepository.GetAuthorById(id);
            return Ok(authorWithId);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthors([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok();
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO authorDTO)
        {
            var authorUpdate = _authorRepository.UpdateAuthorById(id, authorDTO);
            return Ok(authorUpdate);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author == null) return NotFound();
            if (_dbContext.Book_Authors.Any(ba => ba.AuthorId == id))
                return BadRequest("Cannot delete Author with associated Books");
            _dbContext.Authors.Remove(author);
            _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
