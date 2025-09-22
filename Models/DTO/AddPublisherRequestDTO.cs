using WebAPITemple.CustomActionFilter;

namespace WebAPITemple.Models.DTO
{
    public class AddPublisherRequestDTO
    {
        [UniquePublisherName]
        public string Name { set; get; }
    }
}
