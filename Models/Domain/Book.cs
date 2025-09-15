using System.ComponentModel.DataAnnotations;

namespace WebAPITemple.Models.Domain
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string Genre { get; set; }
        public string CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // navigation property
        public int PublisherId { get; set; }
        public Publishers Publisher { get; set; }

        //navigation property for many to many relationship
        public List<Book_Author> BookAuthors { get; set; }
    }
}
