using Microsoft.AspNetCore.Http;

namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class CandidateCreateRequest
    {
        public int ElectionId { get; set; }
        public string CorporateId { get; set; }
        public int SiteId { get; set; }
        public string PhotoBase64 { get; set; }
    }
}
