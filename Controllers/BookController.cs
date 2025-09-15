using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        //khỏi tạo Constructor
        private readonly AppDbContext _dbContext;
        public BookController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Action get all book
        [HttpGet("get-all-book")]
        public IActionResult GetAll() 
        {
            //var AllBooksDomain = _dbContext.Books.ToList();
            var AllBooksDomain = _dbContext.Books;

            //Map domain to DTO
            var AllBookDTO = AllBooksDomain.Select(Book => new Models.DTO.BookDTO()
            {
                Id = Book.Id,
                Title = Book.Title,
                Description = Book.Description,
                IsRead = Book.IsRead,
                DateRead = Book.IsRead ? Book.DateRead : null,
                Rate = Book.IsRead ? Book.Rate : null,
                Genre = Book.Genre,
                CoverUrl = Book.CoverUrl,
                DateAdded = Book.DateAdded,
                PublisherId = Book.PublisherId,
                PublisherName = Book.Publisher != null ? Book.Publisher.Name : "No publisher",
                AuthorNames = Book.BookAuthors != null ? Book.BookAuthors.Select(ba => ba.Author.FullName).ToList() : new List<string>()
            }).ToList();
            //Return DTO
            return Ok(AllBooksDomain);
        }

        //action get book by id
        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (bookDomain == null)
            {
                return NotFound();
            }
            //Map domain to DTO
            var bookDTO = new Models.DTO.BookDTO()
            {
                Id = bookDomain.Id,
                Title = bookDomain.Title,
                Description = bookDomain.Description,
                IsRead = bookDomain.IsRead,
                DateRead = bookDomain.IsRead ? bookDomain.DateRead : null,
                Rate = bookDomain.IsRead ? bookDomain.Rate : null,
                Genre = bookDomain.Genre,
                CoverUrl = bookDomain.CoverUrl,
                DateAdded = bookDomain.DateAdded,
                PublisherId = bookDomain.PublisherId,
                PublisherName = bookDomain.Publisher != null ? bookDomain.Publisher.Name : "No publisher",
                AuthorNames = bookDomain.BookAuthors != null ? bookDomain.BookAuthors.Select(ba => ba.Author.FullName).ToList() : new List<string>()
            };
            //Return DTO
            return Ok(bookDTO);
        }

        // action AddBook 
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] addBookRequestDTO addBookRequestDTO)
        {
            // Map DTO to Domain Model
            var bookDomainModel = new Book
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherId = addBookRequestDTO.PublisherId
            };

            // Use Domain Model to create Book
            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();

            // Add book-author relationships
            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = id
                };
                _dbContext.Book_Authors.Add(bookAuthor);
            }

            // Save all author relationships in one go
            _dbContext.SaveChanges();

            return Ok();
        }

        // action UpdateBookById 
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] addBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);

            if (bookDomain == null)
            {
                return NotFound();
            }

            // Update book properties
            bookDomain.Title = bookDTO.Title;
            bookDomain.Description = bookDTO.Description;
            bookDomain.IsRead = bookDTO.IsRead;
            bookDomain.DateRead = bookDTO.DateRead;
            bookDomain.Rate = bookDTO.Rate;
            bookDomain.Genre = bookDTO.Genre;
            bookDomain.CoverUrl = bookDTO.CoverUrl;
            bookDomain.DateAdded = bookDTO.DateAdded;
            bookDomain.PublisherId = bookDTO.PublisherId;

            // Remove existing author relationships
            var authorDomain = _dbContext.Book_Authors.Where(a => a.BookId == id).ToList();
            if (authorDomain.Any())
            {
                _dbContext.Book_Authors.RemoveRange(authorDomain);
            }

            // Add new author relationships
            foreach (var authorId in bookDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author
                {
                    BookId = id,
                    AuthorId = authorId
                };
                _dbContext.Book_Authors.Add(bookAuthor);
            }

            // Save all changes
            _dbContext.SaveChanges();

            return Ok(bookDTO);
        }

        // action DeleteBookById
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain == null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
           
            return Ok("Deleted successfully");
        }
    }
}
