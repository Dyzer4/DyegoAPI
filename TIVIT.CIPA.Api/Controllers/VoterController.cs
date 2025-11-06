#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
    [Route("voters")]
    [ApiController]
    public class VoterController(IVoterBusiness business) : ControllerBase
    {

        /// <summary>
        ///  Obtem os detalhes de um eleitor
        /// </summary>
        /// <param name="voterId"></param>
        /// <returns>Detalhe do eleitor</returns>
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


        /// <summary>
        ///  Obtem os eleitores
        /// </summary>
        /// <param name="electionId"></param>
        /// <returns>Lista de eleitores</returns>
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

        /// <summary>
        /// Cria um eleitor
        /// </summary>
        /// <param name="request">Dados do eleitor</param>
        /// <returns>Id do eleitor</returns>
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

        /// <summary>
        /// Atualiza um eleitor
        /// </summary>
        /// <param name="id">id do eleitor</param>
        /// <param name="request">dados do eleitor</param>
        /// <returns></returns>
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

        /// <summary>
        /// Ativa ou desativa uma candidato
        /// </summary>
        /// <param name="id">Id da candidato</param>
        /// <param name="active">Ativar ou desativar</param>
        /// <returns></returns>
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
        public async Task<ActionResult> SyncByFileAsync([FromRoute] int electionId)
        {
            var entry = Request.Form.Files.FirstOrDefault(file => file.ContentType == "application/json" && file.Name == "entry");
            var file = Request.Form.Files.FirstOrDefault(file => file.Name == "file");

            if (entry == null)
            {
                return BadRequest("Code da empresa não informado.");
            }
            if (file == null)
            {
                return BadRequest("Chave File não informada.");
            }

            string companyCode;

            using (var reader = new StreamReader(entry.OpenReadStream()))
            {
                var jsonContent = await reader.ReadToEndAsync();
                var jsonDocument = JsonDocument.Parse(jsonContent);
            }

            var fileInMemory = new MemoryStream();
            file.CopyTo(fileInMemory);
            fileInMemory.Seek(0, SeekOrigin.Begin);

            var response = await business.SyncByFileAsync(fileInMemory, electionId);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }

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

