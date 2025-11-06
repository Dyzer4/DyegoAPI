using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Business
{
    public class ActionBusiness(
        IActionRepository repository,
        IUserInfo userInfo) : IActionBusiness
    {
        public async Task<Response<IEnumerable<ActionResponse>>> GetByRoleIdAsync(int profileId)
        {
            var response = new Response<IEnumerable<ActionResponse>>();

            var roles = await repository.GetByProfileIdAsync(profileId, userInfo.Language);

            response.Data = roles.Select(x => new ActionResponse(x.Action.Id, x.Action.Name));

            return response;
        }
    }
}
