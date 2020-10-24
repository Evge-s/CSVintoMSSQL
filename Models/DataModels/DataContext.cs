using Microsoft.EntityFrameworkCore;

namespace CSV_Base.Models.DataModels
{
    public class DataContext : DbContext
    {
        public DbSet<FileCsv> Files { get; set; }
        public DbSet<Person> People { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        { 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=csvappdb;Trusted_Connection=True;");
        }
    }
}
