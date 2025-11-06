#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Mvc;
using TIVIT.CIPA.Api.Attributes;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    //[Role("global_admin", "admin")]
    [Route("sites")]
    [ApiController]
    public class SiteController(ISiteBusiness business) : ControllerBase
    {

        /// <summary>
        ///  Obtem os detalhes de um site
        /// </summary>
        /// <param name="id">id do site</param>
        /// <returns>Detalhe do site</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteResponse>> GetCandidateByIdAsync([FromRoute] int id)
        {
            var response = await business.GetByIdAsync(id);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }



        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SiteResponse>>> GetAllAsync()
        {
            var response = await business.GetAllAsync();

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpGet("actives")]
        public async Task<ActionResult<IEnumerable<SiteResponse>>> GetActiveAsync()
        {
            var response = await business.GetActiveAsync();

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }


    }
}
