using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public interface IBookRepository
    {
        Task<List<BookDTO>> GetAllBooksAsync();  //Async
        Task<BookDTO> GetBookByIdAsync(int id);
        Task<addBookRequestDTO> AddBookAsync(addBookRequestDTO addBookRequestDTO);
        Task<addBookRequestDTO?> UpdateBookByIdAsync(int id, addBookRequestDTO bookDTO);
        Task<Book?> DeleteBookByIdAsync(int id);
    }
}
