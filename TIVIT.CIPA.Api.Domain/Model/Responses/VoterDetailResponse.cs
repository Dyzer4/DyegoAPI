
namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class VoterDetailResponse
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public int SiteId { get; set; }
        public int ProfileId { get; set; }
        public string CorporateId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string CorporatePhone { get; set; }
        public string ContactPhone { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string Area { get; set; }

        public string Department { get; set; }
        public string Token { get; set; }
        public bool HasVoted { get; set; }
        public string ExclusionReason { get; set; }
        public DateTime? ExclusionDate { get; set; }
        public string ExcludedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
