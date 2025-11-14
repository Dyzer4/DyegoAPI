using Microsoft.EntityFrameworkCore;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class SiteRepository(CIPAContext dbContext) : ISiteRepository
    {
        public async Task<Site> GetByIdAsync(int id)
        {
            return await dbContext.Sites.FindAsync(id);
        }

        public async Task<IEnumerable<Site>> GetAllAsync()
        {
            return await dbContext.Sites
                .Include(x => x.Company)
                .ToListAsync();
        }

        public async Task<IEnumerable<Site>> GetActiveAsync()
        {
            return await dbContext.Sites.Where(x=>x.IsActive).ToListAsync();
        }
    }
}
