namespace PhonesAndLaptops;

using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public DbSet<Laptop> Laptops { get; set; }
    public DbSet<MobilePhone> MobilePhones { get; set; }
    public DbSet<Office> Offices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Definiera relationen mellan Office och Assets
        modelBuilder.Entity<Asset>()
            .HasOne(a => a.Office) // Varje Asset har ett Office
            .WithMany(o => o.Assets) // Ett Office har många Assets
            .HasForeignKey(a => a.OfficeId); // ForeignKey i Asset som pekar på Office
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=localhost; port=3306; database=MyDatabaseToCSharp; user=root; password=");
    }
}