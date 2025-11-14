using System.Security.Claims;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Extensions;

namespace TIVIT.CIPA.Api.Security
{
    public class UserInfo : IUserInfo
    {
        private readonly HttpContext _httpContext;

        public UserInfo(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext.HttpContext;
        }

        public string Name => _httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

        public string UniqueName => _httpContext.User.FindFirstValue("unique_name");

        public string Upn => _httpContext.User.FindFirst(ClaimTypes.Upn)?.Value.ToUpper();

        public string AppId => _httpContext.User.FindFirstValue("appid");

        public string Role => _httpContext.GetRole();

        public int SiteID
        {
            get
            {
                var companyIdClaim = _httpContext.User.FindFirstValue("companyId");

                if (int.TryParse(companyIdClaim, out int companyId))
                    return companyId;

                return 0;
            }
        }

        public string Language
        {
            get
            {
                //var language = _httpContext?.Request?.GetTypedHeaders().AcceptLanguage
                //    ?.OrderByDescending(x => x.Quality ?? 1)
                //    .Select(x => x.Value.ToString().Substring(0, 2))
                //    .FirstOrDefault();

                //if (string.IsNullOrEmpty(language) || (language != "es" && language != "pt"))
                //{
                return "pt";
                //}

                //return language;
            }
        }
    }
}
