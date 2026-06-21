namespace SIV.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IQueryable<T>> GetAllAsync();

        Task AddAsync(T entity);
        void Update(T entity);

    }
}
