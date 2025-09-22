using System.ComponentModel.DataAnnotations;
using WebAPITemple.Data;

namespace WebAPITemple.CustomActionFilter
{
    public class UniquePublisherNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var name = value as string;
            if (context.Publishers.Any(p => p.Name == name))
                return new ValidationResult("Publisher name already exists");
            return ValidationResult.Success;
        }
    }
}
