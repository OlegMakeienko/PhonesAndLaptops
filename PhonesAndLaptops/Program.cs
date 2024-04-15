using PhonesAndLaptops;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    static async Task Main(string[] args)
    {
        using (var context = new MyDbContext())
        {
            // var seed = new Seed(context);
            // seed.SeedData();
    
            var manager = new ProductionManager(context);
            await manager.DisplayAssets();


        }
    }
}