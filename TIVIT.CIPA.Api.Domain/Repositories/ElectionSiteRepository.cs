using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Results;
using TIVIT.CIPA.Api.Domain.Repositories.Context;


namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class ElectionSiteRepository:IElectionSiteRepository
    {
        private readonly CIPAContext _dbContext;
        private readonly IUserInfo _userInfo;
        private readonly ILogger<UserRepository> _logger;

        public ElectionSiteRepository(
            CIPAContext dbContext,
            IUserInfo userInfo,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _userInfo = userInfo;
            _logger = logger;
        }

        public async Task<IEnumerable<ElectionSite>> GetByElectionId(int electionId)
        {
            return await _dbContext.ElectionSites.Where(es => es.ElectionId == electionId).ToListAsync();
        }

        public async Task<IEnumerable<Site>> GetSitesByElectionIdAsync(int electionId)
        {
            var result = await (from electionsite in _dbContext.ElectionSites 
                                join site in _dbContext.Sites on electionsite.SiteId equals site.Id
                                where electionsite.ElectionId == electionId
                                select new { electionsite, site }).ToListAsync();

            return result.Select(x => x.site);
        }

        public async Task AddRangeAsync(IEnumerable<ElectionSite> electionSites)
        {
            await _dbContext.ElectionSites.AddRangeAsync(electionSites);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<ElectionSite> electionSites)
        {
            _dbContext.ElectionSites.RemoveRange(electionSites);
            await _dbContext.SaveChangesAsync();
        }
    }
}
