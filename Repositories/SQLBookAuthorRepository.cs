using WebAPITemple.Data;
using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public class SQLBookAuthorRepository : IBookAuthorRepository
    {
        private readonly AppDbContext _context;

        public SQLBookAuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBookAuthor(AddBookAuthorRequestDTO dto)
        {
            var bookAuthor = new Book_Author
            {
                BookId = dto.BookId,
                AuthorId = dto.AuthorId
            };
            _context.Book_Authors.Add(bookAuthor);
            _context.SaveChanges();
        }

        public bool BookExists(int bookId)
        {
            return _context.Books.Any(b => b.Id == bookId);
        }

        public bool AuthorExists(int authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }
    }
}
