
namespace SIV.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork
    {

        private readonly SIVDbContext _context;

        public UnitOfWork(SIVDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
