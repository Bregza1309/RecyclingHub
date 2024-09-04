using Microsoft.EntityFrameworkCore;
using RecyclingHub.Models.DbModels;
namespace RecyclingHub.Data
{
    public static class DbSeedData
    {
        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            HubContext hubContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HubContext>();

            if (hubContext.Database.GetPendingMigrations().Any())
            {
                hubContext.Database.Migrate();
            }
            //check if there are products in the dB
            if(!hubContext.Products.Any())
            {
                //products to be added to the System by default
                Dictionary<string, Dictionary<string, string>> productDictionary = new()
                {
                    {"Copper",new (){ { "110.00", "Copper-Plant-Mineral.webp" } } },
                    {"Brass Heavy",new (){ { "88.50", "brass-scrap.png" } } }
                };

                //map the information in the productDictionary to List of products
                List<Product> products = productDictionary
                        .Select(kvp => new Product { Name = kvp.Key, Price = kvp.Value.Keys.First(), Image = kvp.Value.Values.First() , CompanyName = "Stanlo Bro`s" })
                        .ToList();

                //add the products to the database
                
                //add the products to the inventory
                //var productIds = products.Select(p => p.Id).ToList();
                List<Inventory> inventories = products
                        .Select(product => new Inventory { Product = product  })
                        .ToList();
                await hubContext.Inventory.AddRangeAsync(inventories);
                hubContext.SaveChanges();
            }
        }
    }
}
