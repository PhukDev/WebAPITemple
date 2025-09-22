using System.ComponentModel.DataAnnotations;

namespace WebAPITemple.Models.DTO
{
    public class AddAuthorRequestDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        public string FullName { set; get; }
    }
}
