using System.ComponentModel.DataAnnotations;

namespace WebAPITemple.Models.Domain
{
    public class Authors
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        //navigation property for many to many relationship
        public List<Book_Author> BookAuthors { get; set; }
    }
}
