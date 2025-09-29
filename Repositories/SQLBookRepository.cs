using System.Globalization;
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

        public async Task<List<BookDTO>> GetAllBooksAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allBooks = _dbContext.Books.Select(Books => new BookDTO
            {
                Id = Books.Id,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.DateRead.Value : null,
                Rate = Books.IsRead ? Books.Rate.Value : null,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publisher.Name,
                AuthorNames = Books.BookAuthors.Select(n => n.Author.FullName).ToList()
            }).AsQueryable();
            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title.Contains(filterQuery));
                }
            }
            //sorting 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending ? allBooks.OrderBy(x => x.Title) : allBooks.OrderByDescending(x => x.Title);
                }
            }
            // Paging
            var skipResults = (pageNumber - 1) * pageSize;
            return await allBooks.Skip(skipResults).Take(pageSize).ToListAsync();
            return await allBooks.ToListAsync();

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
