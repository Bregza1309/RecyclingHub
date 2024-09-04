using Microsoft.EntityFrameworkCore;
using RecyclingHub.Data;
using RecyclingHub.Models.DbModels;

namespace RecyclingHub.Dependencies
{
    public class InsectionsService(HubContext hubContext):IHubServices<Insection>
    {
        public HubContext HubContext  = hubContext;

        public async Task AddAsync(Insection item)
        {
            await HubContext.AddAsync(item);
            HubContext.SaveChanges();
        }

        public async Task<List<Insection>> GetAsync(string companyName)
        {
            var insections = await HubContext.Insections.Where(x => x.CompanyName == companyName).OrderByDescending(x => x.Id).ToListAsync();
            return insections;
        }

        public Task<Insection> GetAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(int Id)
        {
            var insection = HubContext.Insections.SingleOrDefault(x => x.Id == Id);
            if (insection != null)
            {
                HubContext.Insections.Remove(insection);
                await HubContext.SaveChangesAsync();
            }
        }

        public Task UpdateAsync(Insection item)
        {
            throw new NotImplementedException();
        }
    }
}
