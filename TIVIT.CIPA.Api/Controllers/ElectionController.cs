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
    [Route("elections")]
    [ApiController]
    public class ElectionController(IElectionBusiness business) : ControllerBase
    {

        /// <summary>
        ///  Obtem os detalhes de uma eleição
        /// </summary>
        /// <param name="electionId"></param>
        /// <returns>Detalhe da eleição</returns>
        [HttpGet()]
#if !DEBUG
        [Action("adm_eleicao", "consultar_eleicao")]
#endif
        public async Task<ActionResult<ElectionResponse>> GetElectionByIdAsync(int electionId)
        {
            var response = await business.GetByIdAsync(electionId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        /// <summary>
        /// Cria uma eleição
        /// </summary>
        /// <param name="request">Dados da eleição</param>
        /// <returns>Id da eleição</returns>
        [HttpPost]
#if !DEBUG
        [Action("adm_eleicao")]
#endif
        public async Task<ActionResult<int>> CreateElectionAsync([FromBody] ElectionCreateRequest request)
        {
            var response = await business.CreateAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Created(string.Empty, response.Data);
        }

        /// <summary>
        /// Atualiza uma eleição
        /// </summary>
        /// <param name="id">id da eleição</param>
        /// <param name="request">dados da eleição</param>
        /// <returns></returns>
        [HttpPut("{id}")]
#if !DEBUG
        [Action("adm_eleicao")]
#endif
        public async Task<ActionResult> UpdateElectionAsync([FromRoute] int id, [FromBody] ElectionUpdateRequest request)
        {
            var response = await business.UpdateAsync(id, request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }

        /// <summary>
        /// Ativa ou desativa uma eleição
        /// </summary>
        /// <param name="id">Id da eleição</param>
        /// <param name="active">Ativar ou desativar</param>
        /// <returns></returns>
        [HttpPut("{id}/active")]
#if !DEBUG
        [Action("adm_eleicao")]
#endif
        public async Task<ActionResult> ChangeElectionActiveAsync([FromRoute] int id, [FromBody] bool active)
        {
            var response = await business.ChangeActiveAsync(active, id);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }
    }
}
