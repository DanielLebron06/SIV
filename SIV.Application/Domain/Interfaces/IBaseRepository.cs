namespace SIV.Application.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();

        Task AddAsync(T entity);
        void Update(T entity);

    }
}
