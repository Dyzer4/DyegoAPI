 using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Business
{
    public class ProfileBusiness(
        IProfileRepository repository) : IProfileBusiness
    {
        public async Task<Response<IEnumerable<ProfileResponse>>> GetAllAsync()
        {
            var response = new Response<IEnumerable<ProfileResponse>>();

            var roles = await repository.GetAllAsync();

            response.Data = roles.Select(x => new ProfileResponse(x.Id, x.Code, x.Name));

            return response;
        }
    }
}
