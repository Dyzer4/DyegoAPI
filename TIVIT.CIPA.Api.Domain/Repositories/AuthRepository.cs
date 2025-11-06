using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CIPAContext _dbContext;

        public AuthRepository(CIPAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserAuth> GetFirstOrDefaultAsync(Expression<Func<UserAuth, bool>> filter)
        {
            return await _dbContext.UsersAuth.FirstOrDefaultAsync(filter);
        }

        public async Task CreateAsync(UserAuth userAuth)
        {
            await _dbContext.UsersAuth.AddAsync(userAuth);
            await _dbContext.SaveChangesAsync();
        }

        async Task IAuthRepository.UpdateAsync(UserAuth userAuth)
        {
            _dbContext.UsersAuth.Update(userAuth);
            await _dbContext.SaveChangesAsync();
        }
    }
}
