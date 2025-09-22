using System.ComponentModel.DataAnnotations;

namespace WebAPITemple.Models.DTO
{
    public class addBookRequestDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Title cannot contain special characters")]
        [MinLength(10)] //bắt buộc độ dài tối thiểu của tiêu đề là 10 ký tự 
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        [Range(0, 5)]   
        public int? Rate { get; set; }
        public string Genre { get; set; }
        public string CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }
        [Required(ErrorMessage = "PublisherID is required")]
        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
