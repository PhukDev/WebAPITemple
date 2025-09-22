using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public interface IBookAuthorRepository
    {
        void AddBookAuthor(AddBookAuthorRequestDTO dto);
        bool BookExists(int bookId);
        bool AuthorExists(int authorId);
    }
}
