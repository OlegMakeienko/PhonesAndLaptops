using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            Console.WriteLine($"Name: {laptop.Name}, " +
                              $"Model: {laptop.Model}, " +
                              $"Price: {laptop.Price:C}, " +
                              $"Production Date: {laptop.ProductionDate.ToShortDateString()}, " +
                              $"Office: {laptop.Office?.Name ?? "No Office"}");
        }

        // Visa mobiltelefoner och deras kontor
        foreach (var phone in mobilePhones)
        {
            Console.WriteLine($"Name: {phone.Name}, " +
                              $"Model: {phone.Model}, " +
                              $"Price: {phone.Price:C}, " +
                              $"Production Date: {phone.ProductionDate.ToShortDateString()}, " +
                              $"Office: {phone.Office?.Name ?? "No Office"}");
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

    public async Task DisplayAssets()
    {
        assets = GetAllPhonesAndLaptops();
        Console.WriteLine("All Assets:");
        {
            Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15} {7,-15:N0}", 
                "Type", "Brand", "Model", "Office", "Purchase Date", "Price in USD", "Currency", "Local Price Today (SEK)");
    
            foreach (var asset in assets.OrderBy(a => a.GetType().Name).ThenBy(a => a.ProductionDate))
            {
                var statusColor = GetEndOfLifeStatus(asset.ProductionDate);
                Console.ForegroundColor = statusColor;
                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15} {7,-15:N0}",
                    asset.GetType().Name, asset.Name, asset.Model, asset.Office.Name, asset.ProductionDate.ToString("d"),
                    asset.Price, GetCurrencyBasedOnOffice(asset), await LocalPriceToday(asset));
            }
            Console.ResetColor();
        }
    }
    
    private string GetCurrencyBasedOnOffice(Asset asset)
    {
        var office = asset.Office.Name;
        if (office == "London")
        {
            return "GBP";
        }
        else if (office == "Stockholm")
        {
            return "SEK";
        }
        else
        {
            return "EUR";
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

    private async Task<double> LocalPriceToday(Asset asset)
    {
        var exchangeRate = await GetExchangeRateToSEK(asset);
        return asset.Price * exchangeRate;
    }
    
    private async Task<double> GetExchangeRateToSEK(Asset asset)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://api.exchangerate-api.com/v4/latest/" + this.GetCurrencyBasedOnOffice(asset));
        var data = await response.Content.ReadAsStringAsync();
        var exchangeRates = JsonConvert.DeserializeObject<ExchangeRateResponse>(data);
        return exchangeRates.Rates["SEK"];
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
