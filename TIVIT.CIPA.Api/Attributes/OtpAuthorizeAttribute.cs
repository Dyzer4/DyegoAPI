using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TIVIT.CIPA.Api.Domain.Providers;

namespace TIVIT.CIPA.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class OtpAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
            var service = context.HttpContext.RequestServices.GetService<OtpProvider>();

            if (service.GetIdentifierFromToken(authorizationHeader) == default) // IsAuhtorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
