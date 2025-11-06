using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Web;
using TIVIT.CIPA.Api.Attributes;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthBusiness _business;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthBusiness business)
        {
            _logger = logger;
            _business = business;
        }

        [HttpPost("otp")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GenerateOtpAsync(AuthRequest request)
        {
            var response = await _business.GenerateOtpAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpPost("token")]
        [OtpAuthorize]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> LogInAsync([FromBody] string otp)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var jwt);

            OtpRequest request = new(jwt, otp);
            var response = await _business.LogInAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpGet("auth-token")]
        [Authorize]
        public async Task<ActionResult<string>> GetAuthTokenAsync()
        {
            var response = await _business.GetAuthTokenAsync();

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> RenewRefreshToken(RenewTokenRequest request)
        {
            var response = await _business.RenewRefreshTokenAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data);
        }

        [HttpPost("password-recover")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPasswordAsync(string username)
        {
            var response = await _business.RecoverPasswordAsync(username);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Accepted();
        }

        [HttpGet("password-recover")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPasswordRecoverKeyAsync([FromQuery] string key)
        {
            var response = await _business.CheckPasswordRecoverKeyAsync(HttpUtility.UrlDecode(key.Replace("+", "%2B")));

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return Ok(response.Data.secondsToExpire);
        }

        [HttpPut("password")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = await _business.ChangePasswordAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return NoContent();
        }

        [HttpPut("first-access-password")]
        [Authorize]
        public async Task<IActionResult> ChangeFirstAccessPasswordAsync(FirstAccessPasswordRequest request)
        {
            var response = await _business.ChangeFirstAccessPasswordAsync(request);

            if (response.HasErrors)
                return BadRequest(response.Messages);

            return NoContent();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await _business.LogOutAsync();

            return NoContent();
        }
    }
}