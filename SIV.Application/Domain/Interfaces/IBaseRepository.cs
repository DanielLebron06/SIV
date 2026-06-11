namespace SIV.Application.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);
        void Update(T entity);

    }
}
