using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebAPITemple.Data
{
    public class BookAuthDbContext : IdentityDbContext
    {
        public BookAuthDbContext(DbContextOptions<BookAuthDbContext> options) :
base(options)
        {
        }
        // tạo phan quyen reader và write cho user  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var readerRoleId = "413b82a0-ad89-4098-b20e-51e8bda18d48"; // sử dụng [guid]::NewGuid() để lấy guid ngẫu nhiên
            var writeRoleId = "0a150b81-bb96-44e0-b864-b208f790f58d";
            base.OnModelCreating(builder);
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name ="Read",
                    NormalizedName="Read".ToUpper()
                },

                new IdentityRole
                {
                    Id = writeRoleId,
                    ConcurrencyStamp = writeRoleId,
                    Name ="Write",
                    NormalizedName="Write".ToUpper()
                }
             };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
