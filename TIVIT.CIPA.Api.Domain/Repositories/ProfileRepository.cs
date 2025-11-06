using Microsoft.EntityFrameworkCore;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class ProfileRepository(CIPAContext dbContext) : IProfileRepository
    {
        public async Task<Profile> GetByIdAsync(int id)
        {
            return await dbContext.Profiles.FindAsync(id);
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await dbContext.Profiles.ToListAsync();
        }
    }
}
