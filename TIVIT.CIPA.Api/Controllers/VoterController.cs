#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TIVIT.CIPA.Api.Attributes;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    //[Role("global_admin", "admin")]
    [Route("voters")]
    [ApiController]
    public class VoterController(IVoterBusiness business, IUserInfo userInfo) : ControllerBase
    {
       
        [HttpGet("{voterId}")]
#if !DEBUG
        [Action("adm_eleitor", "consultar_eleitor","adm_eleicao", "consultar_eleicao")]
#endif
        public async Task<ActionResult<VoterDetailResponse>> GetVoterByIdAsync([FromRoute] int voterId)
        {
            var response = await business.GetByIdAsync(voterId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }


        [HttpGet("by-election/{electionId}")]
#if !DEBUG
        [Action("adm_eleitor", "consultar_eleitor","adm_eleicao", "consultar_eleicao")]
#endif
        public async Task<ActionResult<IEnumerable<VoterDetailResponse>>> GetByElectionIdAsync([FromRoute] int electionId)
        {
            var response = await business.GetByElectionIdAsync(electionId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpPost]
#if !DEBUG
        [Action("adm_eleitor")]
#endif
        public async Task<ActionResult<int>> CreateVoterAsync([FromBody] VoterCreateRequest request)
        {
            var response = await business.CreateAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Created(string.Empty, response.Data);
        }

        [HttpPut("{id}")]
#if !DEBUG
        [Action("adm_eleitor")]
#endif
        public async Task<ActionResult> UpdateVoterAsync([FromRoute] int id, [FromBody] VoterUpdateRequest request)
        {
            var response = await business.UpdateAsync(id, request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }

  
        [HttpPut("{id}/active")]
#if !DEBUG
        [Action("adm_eleitor")]
#endif
        public async Task<ActionResult> ChangeVoterActiveAsync([FromRoute] int id, [FromBody] bool active)
        {
            var response = await business.ChangeActiveAsync(active, id);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }

    
        [HttpPut("sync-file/{electionId}")]
#if !DEBUG
        [Action("adm_eleitor")]
#endif
        public async Task<ActionResult<Response<int>>> SyncByFileAsync([FromRoute] int electionId)
        {
            if (electionId <= 0)
                return BadRequest("ID da eleição inválido.");

            var file = Request.Form.Files.FirstOrDefault(f => f.Name == "file");
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo não fornecido ou vazio.");

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var SiteId = userInfo.SiteID;

                    if (SiteId <= 0)
                        return BadRequest("ID da empresa não disponível no contexto.");

                    var response = await business.SyncByFileAsync(stream, electionId);

                    if (response.HasErrors)
                        return BadRequest(response.Messages);

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new[] { $"Erro ao processar arquivo: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém o template para sincronização de eleitores
        /// </summary>
        /// <returns>Arquivo Excel com template</returns>
        [HttpGet("sync-template")]
        public ActionResult<byte[]> GetSyncTemplate()
        {
            var response = business.GetSyncTemplate();

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return File(response.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SYNC-VOTER-TEMPLATE.xlsx");
        }
    }
}