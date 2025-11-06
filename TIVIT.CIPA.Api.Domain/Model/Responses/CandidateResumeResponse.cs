
namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class CandidateResumeResponse
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public string Site { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }       
    }
}
