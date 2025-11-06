#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Mvc;
using TIVIT.CIPA.Api.Attributes;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [Route("profiles")]
    [ApiController]
    public class ProfileController(IProfileBusiness business) : ControllerBase
    {

        /// <summary>
        /// Obtém todas as roles. 
        /// </summary>
        /// <returns>Lista de roles</returns>
        [HttpGet()]
#if !DEBUG
        [Action("consultar_perfil", "adm_perfil", "adm_user", "consultar_user")]
#endif
        public async Task<ActionResult<IEnumerable<ProfileResponse>>> GetAllAsync()
        {
            var response = await business.GetAllAsync();

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }
    }
}
