using Microsoft.EntityFrameworkCore;
using RecyclingHub.Data;
using RecyclingHub.Models.DbModels;

namespace RecyclingHub.Dependencies
{
    public class DisposalService(HubContext context) : IHubServices<Disposal>
    {
        readonly HubContext Context = context;
        public async Task AddAsync(Disposal item)
        {
            await Context.Disposals.AddAsync(item);
            Context.SaveChanges();
        }

        public async Task<List<Disposal>> GetAsync(string companyName)
        {
            var disposals = await Context.Disposals.Where(x => x.CompanyName == companyName).OrderByDescending(x => x.Id).ToListAsync();
            return disposals;
        }

        public Task<Disposal> GetAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Disposal item)
        {
            throw new NotImplementedException();
        }
    }
}
