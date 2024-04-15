using Microsoft.EntityFrameworkCore;

namespace PhonesAndLaptops;

public class ProductionManager
{
    private List<Asset> assets; // Antag att du har en lista med Asset
    private MyDbContext _context;

    public ProductionManager(MyDbContext context)
    {
        _context = context;
        assets = new List<Asset>(); // Skapa en tom lista för att samla assets

        // Hämta laptops och mobiltelefoner och inkludera deras kontor
        var laptops = _context.Laptops.Include(l => l.Office).ToList();
        var mobilePhones = _context.MobilePhones.Include(m => m.Office).ToList();

        // Lägg till alla laptops i assets-listan
        foreach (var laptop in laptops)
        {
            assets.Add(laptop);
        }

        // Lägg till alla mobiltelefoner i assets-listan
        foreach (var phone in mobilePhones)
        {
            assets.Add(phone);
        }
    }
    
    public void DisplayAllPhonesAndLaptops()
    {
        Console.WriteLine("All Assets:");
        // Hämta alla laptops och mobiltelefoner inklusive deras kontor
        var laptops = _context.Laptops.Include(l => l.Office).ToList();
        var mobilePhones = _context.MobilePhones.Include(m => m.Office).ToList();

        // Visa laptops och deras kontor
        foreach (var laptop in laptops)
        {
            Console.WriteLine($"Name: {laptop.Name}, Model: {laptop.Model}, Price: {laptop.Price:C}, Production Date: {laptop.ProductionDate.ToShortDateString()}, Office: {laptop.Office?.Name ?? "No Office"}");
        }

        // Visa mobiltelefoner och deras kontor
        foreach (var phone in mobilePhones)
        {
            Console.WriteLine($"Name: {phone.Name}, Model: {phone.Model}, Price: {phone.Price:C}, Production Date: {phone.ProductionDate.ToShortDateString()}, Office: {phone.Office?.Name ?? "No Office"}");
        }
    }
    
    public List<Asset> GetAllPhonesAndLaptops()
    {
        var assets = new List<Asset>();

        // Hämta alla laptops och mobiltelefoner inklusive deras kontor
        var laptops = _context.Laptops.Include(l => l.Office).ToList();
        var mobilePhones = _context.MobilePhones.Include(m => m.Office).ToList();

        // Lägg till laptops till assets-listan
        foreach (var laptop in laptops)
        {
            var asset = new Asset
            {
                Name = laptop.Name,
                Model = laptop.Model,
                Price = laptop.Price,
                ProductionDate = laptop.ProductionDate,
                Office = laptop.Office
            };
            assets.Add(asset);
        }

        // Lägg till mobiltelefoner till assets-listan
        foreach (var phone in mobilePhones)
        {
            var asset = new Asset
            {
                Name = phone.Name,
                Model = phone.Model,
                Price = phone.Price,
                ProductionDate = phone.ProductionDate,
                Office = phone.Office
            };
            assets.Add(asset);
        }
        return assets;
    }

    public void DisplayAssets()
    {
        assets = GetAllPhonesAndLaptops();
        Console.WriteLine("All Assets:");
        foreach (var asset in assets)
        {
            var statusColor = GetEndOfLifeStatus(asset.ProductionDate);
            Console.ForegroundColor = statusColor;
            Console.WriteLine($"Name: {asset.Name}, " +
                              $"Model: {asset.Model}, " +
                              $"Price: {asset.Price:C}, " +
                              $"Production Date: {asset.ProductionDate.ToShortDateString()}, " +
                              $"Office: {asset.Office.Name}");
            Console.ResetColor();
        }
    }

    private ConsoleColor GetEndOfLifeStatus(DateTime productionDate)
    {
        var endOfLifeDate = productionDate.AddYears(3);
        var threeMonthsBeforeEndOfLife = endOfLifeDate.AddMonths(-3);
        var sixMonthsBeforeEndOfLife = endOfLifeDate.AddMonths(-6);

        if (DateTime.Now >= threeMonthsBeforeEndOfLife)
        {
            return ConsoleColor.Red;
        }
        else if (DateTime.Now >= sixMonthsBeforeEndOfLife && DateTime.Now < threeMonthsBeforeEndOfLife)
        {
            return ConsoleColor.Yellow;
        }
        else
        {
            return ConsoleColor.White;
        }
    }

    public void UpdateProductionDate(string assetName, DateTime newDate)
    {
        var asset = assets.FirstOrDefault(a => a.Name == assetName);
        if (asset != null)
        {
            asset.ProductionDate = newDate;
            Console.WriteLine($"Updated {asset.Name}'s production date to {newDate.ToShortDateString()}.");
        }
        else
        {
            Console.WriteLine("Asset not found.");
        }
    }
    
    public void DeleteAsset(string assetName)
    {
        var assetToRemove = assets.FirstOrDefault(a => a.Name == assetName);
        if (assetToRemove != null)
        {
            assets.Remove(assetToRemove);
            Console.WriteLine($"Asset {assetName} removed successfully.");
        }
        else
        {
            Console.WriteLine("Asset not found.");
        }
    }
}
