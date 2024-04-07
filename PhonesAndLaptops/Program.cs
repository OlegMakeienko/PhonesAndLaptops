using PhonesAndLaptops;
using Microsoft.EntityFrameworkCore;

using (var context = new MyDbContext())
{
    // // Skapa nya laptoper
    // var macbook = new Laptop
    // {
    //     Name = "MacBook",
    //     Model = "Pro",
    //     Price = 1500,
    //     ProductionDate = DateTime.Now
    // };
    // context.Laptops.Add(macbook);
    //
    //
    // var asus = new Laptop
    // {
    //     Name = "Asus",
    //     Model = "ZenBook",
    //     Price = 1200,
    //     ProductionDate = DateTime.Now
    // };
    // context.Laptops.Add(asus);
    //
    // var lenovo = new Laptop
    // {
    //     Name = "Lenovo",
    //     Model = "ThinkPad",
    //     Price = 1100,
    //     ProductionDate = DateTime.Now
    // };
    // context.Laptops.Add(lenovo);
    //
    // // Skapa nya mobiltelefoner
    // var samsung = new MobilePhone
    // {
    //     Name = "Samsung",
    //     Model = "Galaxy S21",
    //     Price = 900,
    //     ProductionDate = DateTime.Now
    // };
    // context.MobilePhones.Add(samsung);
    //
    // var nokia = new MobilePhone
    // {
    //     Name = "Nokia",
    //     Model = "8.3",
    //     Price = 600,
    //     ProductionDate = DateTime.Now
    // };
    // context.MobilePhones.Add(nokia);
    //
    // var iphone = new MobilePhone
    // {
    //     Name = "iPhone",
    //     Model = "12",
    //     Price = 1000,
    //     ProductionDate = DateTime.Now
    // };
    // context.MobilePhones.Add(iphone);
    //
    // context.SaveChanges();
    
    var laptops = context.Laptops.ToList();
    var mobilePhones = context.MobilePhones.ToList();

    var assets = laptops.Cast<Asset>().Concat(mobilePhones).OrderBy(a => a.Name).ToList();
    ProductionManager productionManager = new ProductionManager(assets);
    
    productionManager.DisplayAllAssets();
    
    productionManager.UpdateProductionDate("Nokia", new DateTime(2010, 10, 10));
    
    productionManager.PrintAssets();
}