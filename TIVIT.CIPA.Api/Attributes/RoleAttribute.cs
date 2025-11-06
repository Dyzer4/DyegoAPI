using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TIVIT.CIPA.Api.Domain.Providers;

namespace TIVIT.CIPA.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class RoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public RoleAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? Array.Empty<string>();
        }

        public string[] AllowedRoles => _allowedRoles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
#if !DEBUG
            context.HttpContext.Request.Headers.TryGetValue("x-security", out var authToken);
            var username = context.HttpContext.User.FindFirst(ClaimTypes.Upn)?.Value;
            var service = context.HttpContext.RequestServices.GetService<AuthTokenProvider>();

            var (isValid, payload) =
                service.ValidatePayload(username, authToken, AllowedRoles, Array.Empty<string>(), out var errorMessage);

            if (!isValid)
                context.Result = new JsonResult(new { message = errorMessage }) { StatusCode = StatusCodes.Status401Unauthorized };
#endif
        }
    }
}
