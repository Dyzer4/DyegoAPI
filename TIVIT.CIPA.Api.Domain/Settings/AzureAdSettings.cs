using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIVIT.CIPA.Api.Domain.Settings
{
    public class AzureAdSettings
    {
        public string Authority { get; set; }
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string Issuer { get; set; }
        public string AppKey { get; set; }
        public string AuthClientId { get; set; }
    }
}
