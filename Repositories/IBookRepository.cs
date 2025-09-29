using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public interface IBookRepository
    {
        Task<List<BookDTO>> GetAllBooksAsync(string? filterOn = null,string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);  //Async
        Task<BookDTO> GetBookByIdAsync(int id);
        Task<addBookRequestDTO> AddBookAsync(addBookRequestDTO addBookRequestDTO);
        Task<addBookRequestDTO?> UpdateBookByIdAsync(int id, addBookRequestDTO bookDTO);
        Task<Book?> DeleteBookByIdAsync(int id);
    }
}
