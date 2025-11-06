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
    //[Role("global_admin", "admin", "rh")]
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _business;

        public UserController(IUserBusiness business)
        {
            _business = business;
        }

        /// <summary>
        /// Obtem os detalhes de um usuário
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Detalhe do usuário</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserDetailResponse>>> GetUserDetailAsync([FromRoute] int id)
        {
            var response = await _business.GetByIdAsync(id);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        /// <param name="request">Dados do usuário</param>
        /// <returns>Id do usuário</returns>
        [HttpPost]
#if !DEBUG
        [Action("adm_user")]
#endif

        public async Task<ActionResult<int>> CreateUserAsync([FromBody] UserCreateRequest request)
        {
            var response = await _business.CreateAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Created(string.Empty, response.Data);
        }

        /// <summary>
        /// Atualiza um usuáiro
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <param name="request">dados do usuário</param>
        /// <returns></returns>
        [HttpPut("{id}")]
#if !DEBUG
        [Action("adm_user")]
#endif
        public async Task<ActionResult> UpdateUserAsync([FromRoute] int id, [FromBody] UserUpdateRequest request)
        {
            var response = await _business.UpdateAsync(id, request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }

        /// <summary>
        /// Ativa ou desativa um usuário
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <param name="active">Ativar ou desativar</param>
        /// <returns></returns>
        [HttpPut("{id}/active")]
#if !DEBUG
        [Action("adm_user")]
#endif
        public async Task<ActionResult> ChangeUserActiveAsync([FromRoute] int id, [FromBody] bool active)
        {
            var response = await _business.ChangeActiveAsync(active, id);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok();
        }


    }
}
