using Microsoft.EntityFrameworkCore;

namespace PhonesAndLaptops;

public class AssetService
{
    private MyDbContext _context;

    public AssetService(MyDbContext context) {
        _context = context;
    }
    public List<Asset> GetAllAssets() {
        var assets = new List<Asset>();
        var laptops = _context.Laptops.Include(l => l.Office).ToList();
        var mobilePhones = _context.MobilePhones.Include(m => m.Office).ToList();

        assets.AddRange(laptops);
        assets.AddRange(mobilePhones);

        return assets;
    }
    
    public void AddAsset(Asset asset)
    {
        switch (asset)
        {
            case Laptop laptop:
                _context.Laptops.Add(laptop);
                break;
            case MobilePhone mobilePhone:
                _context.MobilePhones.Add(mobilePhone);
                break;
            default:
                throw new ArgumentException("Unknown asset type");
        }
        _context.SaveChanges();
    }
    
    public void UpdateAsset(Asset asset)
    {
        _context.Entry(asset).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public Asset FindAssetByName(string name)
    {
        var assets = GetAllAssets();
        return assets.FirstOrDefault(asset => asset.Name == name);
    }

    public void DeleteAsset(Asset asset)
    {
        _context.Entry(asset).State = EntityState.Deleted;
        _context.SaveChanges();
    }
}