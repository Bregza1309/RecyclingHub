using Microsoft.EntityFrameworkCore;
using RecyclingHub.Data;
using RecyclingHub.Models.DbModels;

namespace RecyclingHub.Dependencies
{
    public class AcquisitionService(HubContext context) : IHubServices<Aquisition>
    {
        readonly HubContext hubContext = context;

        public async Task AddAsync(Aquisition item)
        {
            await hubContext.AddAsync(item);
            hubContext.SaveChanges();
        }

        public async Task<List<Aquisition>> GetAsync(string companyName)
        {
            var acquisitions = await  hubContext.Acquisitions.Where(x => x.CompanyName == companyName).OrderByDescending(a => a.Id).ToListAsync();
            return acquisitions;
        }

        public Task<Aquisition> GetAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Aquisition item)
        {
            throw new NotImplementedException();
        }
    }
}
