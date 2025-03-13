using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
            // dbcontextoptions is a class that is used to configure the dbcontext
            // base is used to call the constructor of the parent class which is IdentityDbContext in this case and pass the options to it 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seeding the roles
            modelBuilder.Entity<IdentityRole>();
            var readerRoleId = "92161203-ed22-42d2-b5f8-c87d0b648049";
            var writerRoleId = "909f2ecc-2cd5-41f7-bb36-562c6762f71a";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = "92161203-ed22-42d2-b5f8-c87d0b648049",
                    Name = "Reader",
                    NormalizedName = "READER",
                    ConcurrencyStamp = readerRoleId

                },
                new IdentityRole()
                {
                    Id = "909f2ecc-2cd5-41f7-bb36-562c6762f71a",
                    Name = "Writer",
                    NormalizedName = "WRITER",
                    ConcurrencyStamp = writerRoleId // concurrency stamp is used to handle concurrency issues like when two users try to update the same record at the same time 
                    // so the system can check if the concurrency stamp is the same and if it is not then it will throw an exception
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
