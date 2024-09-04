using RecyclingHub.Models.DbModels;
using RecyclingHub.Data;
using Microsoft.EntityFrameworkCore;

namespace RecyclingHub.Dependencies
{
    public class ProductsService : IHubServices<Product>
    {
        HubContext HubContext { get; set; }
        public ProductsService(HubContext hubContext)
        {
            HubContext = hubContext;
        }
        public async Task AddAsync(Product item)
        {
            await HubContext.Products.AddAsync(item);
            HubContext.SaveChanges();
        }

        public async Task<List<Product>> GetAsync(string companyName)
        {
            return await HubContext.Products.Where(x => x.CompanyName == companyName).ToListAsync();
        }

        public async Task<Product> GetAsync(int Id)
        {
            return await HubContext.Products.SingleAsync(x => x.Id == Id);
        }

        public async Task RemoveAsync(int Id)
        {
            Product? product = await HubContext.Products.SingleOrDefaultAsync(x => x.Id == Id);
            if(product != null)
            {
                HubContext.Products.Remove(product);
                HubContext.SaveChanges() ;
            }
        }

        public async Task UpdateAsync(Product item)
        {
            HubContext.Update(item);
            await HubContext.SaveChangesAsync();
        }
    }
}
