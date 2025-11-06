using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CIPAContext _dbContext;
        private readonly IUserInfo _userInfo;
        private readonly ILogger<UserRepository> _logger;

        public CandidateRepository(
            CIPAContext dbContext,
            IUserInfo userInfo,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _userInfo = userInfo;
            _logger = logger;
        }

        public async Task<Candidate> GetByIdAsync(int id)
        {
            return await _dbContext.Candidates
                .Include(x => x.Site)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Candidate>> GetByElectionIdAsync(int electionId)
        {
            return await _dbContext.Candidates
                .Include(x => x.Site)
                .Where(x => x.ElectionId == electionId).ToListAsync();
        }

        public async Task<IEnumerable<Candidate>> SearchAsync(string name, int electionId, int? siteId = null, string? registry = null, string? departament = null)
        {
            var query = _dbContext.Candidates
                .Include(c => c.Voter)
                .Where(c => c.IsActive)
                .Where(c => c.ElectionId == electionId);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            if (siteId.HasValue)
                query = query.Where(c => c.SiteId == siteId.Value);

            if (!string.IsNullOrWhiteSpace(registry))
                query = query.Where(c => c.Voter.Registry == registry);

            if (!string.IsNullOrWhiteSpace(departament))
                query = query.Where(c => c.Department == departament);

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            _dbContext.Candidates.Update(candidate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Candidate candidate)
        {
            await _dbContext.Candidates.AddAsync(candidate);
            await _dbContext.SaveChangesAsync();
        }

        public bool ExistsById(int id)
        {
            return _dbContext.Candidates.Any(x => x.Id == id);
        }
    }
}