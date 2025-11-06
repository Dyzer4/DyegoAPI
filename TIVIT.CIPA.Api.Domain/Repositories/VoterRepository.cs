using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class VoterRepository : IVoterRepository
    {
        private readonly CIPAContext _dbContext;
        private readonly IUserInfo _userInfo;
        private readonly ILogger<UserRepository> _logger;

        public VoterRepository(
            CIPAContext dbContext,
            IUserInfo userInfo,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _userInfo = userInfo;
            _logger = logger;
        }

        public async Task<Voter> GetByIdAsync(int id)
        {
            return await _dbContext.Voters.FindAsync(id);
        }

        public async Task<IEnumerable<Voter>> GetByElectionIdAsync(int electionId)
        {
            return await _dbContext.Voters.Where(x => x.ElectionId == electionId).ToListAsync();
        }

        public async Task UpdateAsync(Voter voter)
        {
            _dbContext.Voters.Update(voter);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Voter voter)
        {
            await _dbContext.Voters.AddAsync(voter);
            await _dbContext.SaveChangesAsync();
        }
               
        public bool ExistsById(int id)
        {
            return _dbContext.Voters.Any(x => x.Id == id);
        }
    }
}
