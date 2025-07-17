using Microsoft.EntityFrameworkCore;
using Write_from_csv_to_database.Entities;

namespace Write_from_csv_to_database;

internal class ApplicationContext : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<Pet> Pets { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=appdatabase;Trusted_Connection=True;");
        base.OnConfiguring(optionsBuilder);
    }
}
