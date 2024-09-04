using Microsoft.EntityFrameworkCore;
using RecyclingHub.Data;
using RecyclingHub.Models.DbModels;
using System.ComponentModel;

namespace RecyclingHub.Dependencies
{
    public class InventoryService(HubContext context):IHubServices<Inventory>
    {
        readonly HubContext hubContext = context;

        public async Task AddAsync(Inventory item)
        {
            await hubContext.Inventory.AddAsync(item);
            hubContext.SaveChanges();
        }

        public async Task<List<Inventory>> GetAsync(string companyName)
        {
            var inventories = await hubContext.Inventory.Where(x => x.CompanyName == companyName).ToListAsync();
            return inventories;
        }

        public Task<Inventory> GetAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Inventory item)
        {
            hubContext.Inventory.Update(item);
            await hubContext.SaveChangesAsync();
        }
    }
}
