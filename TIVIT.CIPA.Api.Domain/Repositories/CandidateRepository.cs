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
                .Include(x=>x.Site)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<IEnumerable<Candidate>> GetByElectionIdAsync(int electionId)
        {
            return await _dbContext.Candidates
                .Include(x => x.Site)
                .Where(x => x.ElectionId == electionId).ToListAsync();
        }

        public async Task<IEnumerable<Candidate>> SearchAsync(string name, int? electionId = null, bool? isActive = null, int? siteId = null)
        {
            var query = _dbContext.Candidates
                .AsQueryable();

            if (electionId.HasValue)
                query = query.Where(c => c.ElectionId == electionId.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive.Value);

            if (siteId.HasValue)
                query = query.Where(c => c.SiteId == siteId.Value);

            //opção com expressão lambda:
            //var result = await (
            //from candidate in _dbContext.Candidates

            //where candidate.ElectionId == electionId
            //   && (string.IsNullOrEmpty(name) || candidate.Name.Contains(name))
            //   && (!isActive.HasValue || candidate.IsActive == isActive.Value)
            //   && (!siteId.HasValue || candidate.SiteId == siteId.Value)
            //select new Candidate
            //{
            //    Id = candidate.Id,
            //    ElectionId = candidate.ElectionId,
            //    CorporateId = candidate.CorporateId,
            //    Name = candidate.Name,
            //    Area = candidate.Area,
            //    SiteId = candidate.SiteId,
            //    IsActive = candidate.IsActive,
            //    PhotoBase64 = candidate.PhotoBase64
            //}).ToListAsync();


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
