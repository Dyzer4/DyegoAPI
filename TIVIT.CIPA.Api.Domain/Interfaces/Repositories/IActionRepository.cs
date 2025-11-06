using TIVIT.CIPA.Api.Domain.Model.Results;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public  interface IActionRepository
    {
        Task<IEnumerable<ActionResult>> GetByProfileIdAsync(int profileId, string language);
    }
}
