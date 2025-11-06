using Microsoft.EntityFrameworkCore;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model.Results;
using TIVIT.CIPA.Api.Domain.Repositories.Context;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class ActionRepository(CIPAContext dbContext) : IActionRepository
    {
        public async Task<IEnumerable<ActionResult>> GetByProfileIdAsync(int profileId, string language)
        {
            var result = await (from act in dbContext.Actions
                                join pa in dbContext.ProfileActions on act.Id equals pa.ActionId
                                where pa.ProfileId == profileId
                                select new { act }).ToListAsync();

            return result.Select(x => new ActionResult(x.act));
        }
    }
}
