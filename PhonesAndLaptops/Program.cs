using PhonesAndLaptops;
using Microsoft.EntityFrameworkCore;

using (var context = new MyDbContext())
{
    // var seed = new Seed(context);
    // seed.SeedData();
    
    var manager = new ProductionManager(context);
    manager.DisplayAssets();


}