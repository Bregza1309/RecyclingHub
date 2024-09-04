namespace RecyclingHub.Dependencies
{
    public interface IHubServices<T>
    {
        Task<List<T>> GetAsync(string companyName);
        Task<T> GetAsync(int Id);

        Task AddAsync(T item);
        Task RemoveAsync(int Id);
         Task UpdateAsync(T item);

        
    }
}
