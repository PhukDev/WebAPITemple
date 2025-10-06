using System.ComponentModel.DataAnnotations;

namespace WebAPITemple.Models.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
