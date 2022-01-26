using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Repository.Implementation
{
    abstract public class Save
    {
        private readonly DbContext _context;

        public Save(DbContext context)
        {
            _context = context;
        }
        public async Task<int> RepoSaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
