using EFContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFContext
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Drive> Drives { get; set; }
        public DbSet<Entities.File> ODFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "DB");
    }

}
