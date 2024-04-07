namespace PhonesAndLaptops;

public class ProductionManager
{
    private List<Asset> assets; // Antag att du har en lista med Asset

    public ProductionManager(List<Asset> assets)
    {
        this.assets = assets;
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
    
    public void DisplayAllAssets()
    {
        Console.WriteLine("All Assets:");
        foreach (var asset in assets)
        {
            Console.WriteLine($"Name: {asset.Name}, " +
                              $"Model: {asset.Model}, " +
                              $"Price: {asset.Price:C}, " +
                              $"Production Date: {asset.ProductionDate.ToShortDateString()}");
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
    
    public void PrintAssets()
    {
        foreach (var asset in assets)
        {
            string status = GetAssetStatus(asset);
            if (status == "Old gadget must be changed!")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ResetColor();
            }
            Console.WriteLine($"{asset.Name} ({status})");
        }
        Console.ResetColor();
    }
    
    private string GetAssetStatus(Asset asset)
    {
        TimeSpan timeSinceProduction = DateTime.Now - asset.ProductionDate;
        int years = timeSinceProduction.Days / 365;
        int months = (timeSinceProduction.Days % 365) / 30;

        if (years >= 3 && months >= 3)
        {
            return "Old gadget must be changed!"; // Röd status
        }
        else
        {
            return "Normal"; // Grön status
        }
    }
}
