#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif
using DocumentFormat.OpenXml.Bibliography;
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
    [Route("candidates")]
    [ApiController]
    public class CandidateController(ICandidateBusiness business) : ControllerBase
    {

        /// <summary>
        ///  Obtem os detalhes de uma candidato
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns>Detalhe da candidato</returns>
        [HttpGet("{candidateId}")]
#if !DEBUG
        [Action("adm_candidato", "consultar_candidato","adm_eleicao", "consultar_eleicao")]
#endif
        public async Task<ActionResult<CandidateDetailResponse>> GetCandidateByIdAsync([FromRoute] int candidateId)
        {
            var response = await business.GetByIdAsync(candidateId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }


        /// <summary>
        ///  Obtem os candidatos
        /// </summary>
        /// <param name="electionId"></param>
        /// <returns>Lista de candidatos</returns>
        [HttpGet("by-election/{electionId}")]
#if !DEBUG
        [Action("adm_candidato", "consultar_candidato","adm_eleicao", "consultar_eleicao")]
#endif
        public async Task<ActionResult<IEnumerable<CandidateDetailResponse>>> GetByElectionIdAsync([FromRoute] int electionId)
        {
            var response = await business.GetByElectionIdAsync(electionId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpGet("search")]
#if !DEBUG
        [Action("adm_candidato", "consultar_candidato")]
#endif
        public async Task<ActionResult<IEnumerable<CandidateResumeResponse>>> SearchCandidateAsync(
            [FromQuery] int electionId,
            [FromQuery] string name,
            [FromQuery] string? corporateid,
            [FromQuery] string? area,
            [FromQuery] int? siteId
            )
        {
            var response = await business.SearchCandidateAsync(name, electionId, siteId, corporateid, area);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }



        /// <summary>
        /// Cria uma candidato
        /// </summary>
        /// <param name="request">Dados da candidato</param>
        /// <returns>Id da candidato</returns>
        [HttpPost]
#if !DEBUG
        [Action("adm_candidato")]
#endif
        public async Task<ActionResult<int>> CreateCandidateAsync([FromBody] CandidateCreateRequest request)
        {
            var response = await business.CreateAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Created(string.Empty, response.Data);
        }

        /// <summary>
        /// Atualiza uma candidato
        /// </summary>
        /// <param name="id">id da candidato</param>
        /// <param name="request">dados da candidato</param>
        /// <returns></returns>
        [HttpPut("{id}")]
#if !DEBUG
        [Action("adm_candidato")]
#endif
        public async Task<ActionResult> UpdateCandidateAsync([FromRoute] int id, [FromBody] CandidateUpdateRequest request)
        {
            var response = await business.UpdateAsync(id, request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }

        /// <summary>
        /// Ativa ou desativa uma candidato
        /// </summary>
        /// <param name="id">Id da candidato</param>
        /// <param name="active">Ativar ou desativar</param>
        /// <returns></returns>
        [HttpPut("{id}/active")]
#if !DEBUG
        [Action("adm_candidato")]
#endif
        public async Task<ActionResult> ChangeCandidateActiveAsync([FromRoute] int id, [FromBody] bool active)
        {
            var response = await business.ChangeActiveAsync(active, id);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }
    }
}
