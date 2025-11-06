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
    //[Role("global_admin", "admin")]
    [Route("actions")]
    [ApiController]
    public class ActionController(IActionBusiness business) : ControllerBase
    {

        /// <summary>
        /// Obtém todas as actions por perfil. 
        /// </summary>
        /// <param name="profileId">id do perfil</param>
        /// <returns>lista de actions</returns>
        [HttpGet()]
#if !DEBUG
        [Action("consultar_perfil", "adm_perfil", "adm_user", "consultar_user")]
#endif
        public async Task<ActionResult<IEnumerable<ActionResponse>>> GetByRoleIdAsync(int profileId)
        {
            var response = await business.GetByRoleIdAsync(profileId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }
    }
}
