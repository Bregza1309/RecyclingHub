using RecyclingHub.Models.DbModels;
using RecyclingHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace RecyclingHub.Dependencies
{
    public class SalesService(HubContext hubContext) : IHubServices<Sale>
    {
        HubContext hubContext = hubContext;
        

        public async Task AddAsync(Sale item)
        {
            await hubContext.AddAsync(item);
            hubContext.SaveChanges();
            
        }

       

        public async Task<List<Sale>> GetAsync(string companyName)
        {
            var sales = await hubContext.Sales.Where(x => x.CompanyName == companyName).OrderByDescending(s => s.Date).ToListAsync();
            return sales.Count > 0 ? sales : new();
            
        }

        public async Task<Sale> GetAsync(int Id)
        {
            var sale = await hubContext.Sales.SingleOrDefaultAsync( s => s.Id == Id) ?? new();
            return sale;
        }

        public async Task RemoveAsync(int Id)
        {
            var sale = await hubContext.Sales.SingleOrDefaultAsync(s => s.Id == Id);
            if (sale != null)
            {
                hubContext.Sales.Remove(sale);
                hubContext.SaveChanges();
            }
        }

        public  Task UpdateAsync(Sale item)
        {
            throw new NotImplementedException();
        }
    }
}
