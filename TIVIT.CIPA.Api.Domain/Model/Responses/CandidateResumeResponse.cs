
namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class CandidateResumeResponse
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public string CorporateId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public bool IsActive { get; set; }       
    }
}
