namespace PhonesAndLaptops;

using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public DbSet<Laptop> Laptops { get; set; }
    public DbSet<MobilePhone> MobilePhones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=localhost; port=3306; database=MyDatabaseToCSharp; user=root; password=");
    }
}