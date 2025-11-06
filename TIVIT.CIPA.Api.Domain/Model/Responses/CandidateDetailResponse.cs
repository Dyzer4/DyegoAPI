namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class CandidateDetailResponse
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string? PhotoBase64 { get; set; }
        public bool IsActive { get; set; }
    }
}
