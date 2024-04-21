using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace PhonesAndLaptops;

public class ProductionManager
{
    private List<Asset> _assets;
    private readonly AssetService _assetService;
    
    public ProductionManager(MyDbContext context) {
        _assetService = new AssetService(context);
    }
    public async Task DisplayAssets()
    {
        _assets = _assetService.GetAllAssets();
        Console.WriteLine("All Assets:");
        {
            Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15} {7,-15:N0}", 
                "Type", "Brand", "Model", "Office", "Purchase Date", "Price in USD", "Currency", "Local Price Today (SEK)");
    
            foreach (var asset in _assets.OrderBy(a => a.GetType().Name).ThenBy(a => a.ProductionDate))
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
    
    public void CreateAsset()
    {
        Console.WriteLine("Enter the type of the asset (MobilePhone or Laptop):");
        string assetType = Console.ReadLine();

        Console.WriteLine("Enter the name of the asset:");
        string name = Console.ReadLine();

        Console.WriteLine("Enter the model:");
        string model = Console.ReadLine();

        Console.WriteLine("Enter the price:");
        int price = int.Parse(Console.ReadLine());  // Assume user inputs a correct numeric value

        Console.WriteLine("Enter Office ID:");
        int officeId = int.Parse(Console.ReadLine());  // Assume user inputs a correct numeric value

        Asset newAsset;

        switch (assetType)
        {
            case "MobilePhone":
                newAsset = new MobilePhone
                {
                    Name = name,
                    Model = model,
                    Price = price,
                    ProductionDate = DateTime.Now,
                    OfficeId = officeId
                };
                break;
            case "Laptop":
                newAsset = new Laptop
                {
                    Name = name,
                    Model = model,
                    Price = price,
                    ProductionDate = DateTime.Now,
                    OfficeId = officeId
                };
                break;
            default:
                Console.WriteLine("Invalid asset type. Please enter either 'MobilePhone' or 'Laptop'.");
                return;
        }

        _assetService.AddAsset(newAsset);

        Console.WriteLine("Asset added!");
    }
    
    public void UpdateAssetProductionDate(string assetName, DateTime newDate)
    {
        Asset asset = _assetService.FindAssetByName(assetName);
        
        asset.ProductionDate = newDate;

        _assetService.UpdateAsset(asset);

        Console.WriteLine($"Update {asset.Name}s production date to {newDate.ToShortDateString()}.");
    }

    
    public void DeleteAsset(string assetName)
    {
        Asset asset = _assetService.FindAssetByName(assetName);
        
        if (asset != null)
        {
            _assetService.DeleteAsset(asset);
            Console.WriteLine($"Asset {assetName} removed successfully.");
        }
        else
        {
            Console.WriteLine("Asset not found.");
        }
    }
}
