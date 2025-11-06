using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("external-auth")]
    public class ExternalAuthController(
        ILogger<AuthController> logger,
        IAuthBusiness business) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly IAuthBusiness _business = business;

        [HttpPost("token")]
        public async Task<ActionResult<ExternalAuthResponse>> GenerateServiceTokenAsync([FromBody] ExternalAuthRequest request)
        {
            var response = await _business.GenerateServiceTokenAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Created(string.Empty, response.Data);
        }
    }
}