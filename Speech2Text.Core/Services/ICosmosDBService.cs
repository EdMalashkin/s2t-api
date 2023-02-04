namespace Speech2Text.Core.Services
{
    public interface ICosmosDbService<T>
    {
        Task<IEnumerable<T>> GetMultipleAsync(string query);
        Task<T> GetAsync(string id);
        Task AddAsync(string id, T item);
        Task UpdateAsync(string id, T item);
        Task DeleteAsync(string id);
    }
}
