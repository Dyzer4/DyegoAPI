using DocumentFormat.OpenXml.Bibliography;
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
                .Include(c => c.Voter)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Voter> GetByCorporateIdandElectionIdAsync( int electionId, string corporateId)
        {
            var query = _dbContext.Voters
                .Include(v => v.Site)
                .Where(c => c.IsActive)
                .Where(c => c.ElectionId == electionId);

            if (!string.IsNullOrWhiteSpace(corporateId))
                query = query.Where(c => c.CorporateId == corporateId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Candidate>> GetByElectionIdAsync(int electionId)
        {
            return await _dbContext.Candidates
                .Include(x => x.Voter)
                .ThenInclude(v => v.Election)
                .Include(s => s.Voter.Site)
                .Where(x => x.IsActive)
                .Where(x => x.Voter.ElectionId == electionId)
                .OrderBy(x => x.Voter.Name.Trim().ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Candidate>> SearchAsync(string name, int electionId, int? siteId = null, string? corporateId = null, string? department = null)
        {
            var query = _dbContext.Candidates
                .Include(c => c.Voter)
                .Include(c => c.Voter.Site)
                .Where(c => c.IsActive)
                .Where(c => c.Voter.ElectionId == electionId);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Voter.Name.Contains(name));

            if (siteId.HasValue)
                query = query.Where(c => c.Voter.SiteId == siteId.Value);

            if (!string.IsNullOrWhiteSpace(corporateId))
                query = query.Where(c => c.Voter.CorporateId == corporateId);

            if (!string.IsNullOrWhiteSpace(department))
                query = query.Where(c => c.Voter.Department == department);

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