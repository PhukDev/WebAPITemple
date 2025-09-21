using Microsoft.EntityFrameworkCore;
using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BookDTO>> GetAllBooksAsync()
        {
            var allBooks = await _dbContext.Books  // Sử dụng await ToListAsync()
                .Select(book => new BookDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.IsRead ? book.DateRead.Value : null,
                    Rate = book.IsRead ? book.Rate.Value : null,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher.Name,
                    AuthorNames = book.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                }).ToListAsync();  // Async query

            return allBooks;
        }

        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            var bookWithDomain = await _dbContext.Books.Where(n => n.Id == id)
                .Select(book => new BookDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.DateRead,
                    Rate = book.Rate,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher.Name,
                    AuthorNames = book.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                }).FirstOrDefaultAsync();  // Async FirstOrDefault

            return bookWithDomain ?? new BookDTO();  
        }

        public async Task<addBookRequestDTO> AddBookAsync(addBookRequestDTO addBookRequestDTO)
        {
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

            _dbContext.Books.Add(bookDomainModel);
            await _dbContext.SaveChangesAsync();  // Async save

            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = id
                };
                _dbContext.Book_Authors.Add(bookAuthor);
                await _dbContext.SaveChangesAsync();
            }
            return addBookRequestDTO;
        }

        public async Task<addBookRequestDTO?> UpdateBookByIdAsync(int id, addBookRequestDTO bookDTO)
        {
            var bookDomain = await _dbContext.Books.FirstOrDefaultAsync(n => n.Id == id);  // Async
            if (bookDomain != null)
            {
                bookDomain.Title = bookDTO.Title;
                bookDomain.Description = bookDTO.Description;
                bookDomain.IsRead = bookDTO.IsRead;
                bookDomain.DateRead = bookDTO.DateRead;
                bookDomain.Rate = bookDTO.Rate;
                bookDomain.Genre = bookDTO.Genre;
                bookDomain.CoverUrl = bookDTO.CoverUrl;
                bookDomain.DateAdded = bookDTO.DateAdded;
                bookDomain.PublisherId = bookDTO.PublisherId;
                await _dbContext.SaveChangesAsync();  // Async
            }

            var authorDomains = await _dbContext.Book_Authors.Where(a => a.BookId == id).ToListAsync();  // Async
            if (authorDomains != null && authorDomains.Any())
            {
                _dbContext.Book_Authors.RemoveRange(authorDomains);
                await _dbContext.SaveChangesAsync();
            }

            foreach (var authorid in bookDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorid
                };
                _dbContext.Book_Authors.Add(bookAuthor);
                await _dbContext.SaveChangesAsync();
            }
            return bookDTO;
        }

        public async Task<Book?> DeleteBookByIdAsync(int id)
        {
            var bookDomain = await _dbContext.Books.FirstOrDefaultAsync(n => n.Id == id);  // Async
            if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                await _dbContext.SaveChangesAsync();  // Async
            }
            return bookDomain;
        }
    }
}
