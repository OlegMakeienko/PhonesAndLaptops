namespace PhonesAndLaptops;

public class Seed
{
    private readonly MyDbContext _context;
    public Seed(MyDbContext context)
    {
        _context = context;
    }

    public void SeedData()
    {

        // Skapa laptops
        var macBook = new Laptop { Name = "MacBook", Model = "Pro", Price = 1500m, ProductionDate = DateTime.Now };
        var asus = new Laptop { Name = "Asus", Model = "ZenBook", Price = 1000m, ProductionDate = DateTime.Now };
        var lenovo = new Laptop
            { Name = "Lenovo", Model = "ThinkPad", Price = 1200m, ProductionDate = DateTime.Now };

        // Skapa mobiltelefoner
        var iphone = new MobilePhone
            { Name = "Iphone", Model = "13", Price = 1000m, ProductionDate = DateTime.Now };
        var samsung = new MobilePhone
            { Name = "Samsung", Model = "Galaxy S21", Price = 800m, ProductionDate = DateTime.Now };
        var nokia = new MobilePhone { Name = "Nokia", Model = "3310", Price = 50m, ProductionDate = DateTime.Now };

        // Skapa kontor
        var london = new Office { Name = "London", Location = "London", Price = 5000 };
        var brussels = new Office { Name = "Brussels", Location = "Brussels", Price = 4000 };
        var stockholm = new Office { Name = "Stockholm", Location = "Stockholm", Price = 6000 };

        // Lägg till tillgångar i kontor
        london.Assets = new List<Asset> { macBook, iphone };
        brussels.Assets = new List<Asset> { asus, samsung };
        stockholm.Assets = new List<Asset> { lenovo, nokia };

        // Lägg till objekt i DbContext
        if (!_context.Laptops.Any())
        {
            _context.Laptops.AddRange(new List<Laptop> { macBook, asus, lenovo });
        }

        if (!_context.MobilePhones.Any())
        {
            _context.MobilePhones.AddRange(new List<MobilePhone> { iphone, samsung, nokia });
        }
        if (!_context.Offices.Any())
        {
            _context.Offices.AddRange(new List<Office> { london, brussels, stockholm });
        }

        // Spara ändringar
        _context.SaveChanges();
    }
}