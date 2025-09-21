using WebAPITemple.Models.Domain;
using WebAPITemple.Models.DTO;

namespace WebAPITemple.Repositories
{
    public interface IAuthorRepository
    {
        List<AuthorDTO> GetAllAuthors();  
        AuthorNoIdDTO GetAuthorById(int id);
        AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);
        AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO);
        Authors DeleteAuthorById(int id);
    }
}
