using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Entity;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class ElectionRepository : IElectionRepository
    {
        private readonly CIPAContext _dbContext;
        private readonly IUserInfo _userInfo;
        private readonly ILogger<UserRepository> _logger;

        public ElectionRepository(
            CIPAContext dbContext,
            IUserInfo userInfo,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _userInfo = userInfo;
            _logger = logger;
        }

        public async Task<Election> GetByIdAsync(int id)
        {
            return await _dbContext.Elections.FindAsync(id);
        }

        public async Task<IEnumerable<Election>> GetAllAsync()
        {
            return await _dbContext.Elections.ToListAsync();
        }

        public async Task UpdateAsync(Election election)
        {
            _dbContext.Elections.Update(election);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Election election)
        {
            await _dbContext.Elections.AddAsync(election);
            await _dbContext.SaveChangesAsync();
        }

        public bool ExistsActiveByName(string name, int? ignoreId = null)
        {
            var query = _dbContext.Elections
                .Where(x => x.Name == name && x.IsActive);

            if (ignoreId.HasValue)
                query = query.Where(x => x.Id != ignoreId.Value);

            return query.Any();
        }

        public bool ExistsById(int id)
        {
            return _dbContext.Elections.Any(x => x.Id == id);
        }
    }
}
