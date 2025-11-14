namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class CandidateDetailResponse
    {
        public int Id { get; set; }
        public string ElectionName { get; set; }
        public string SiteName { get; set; }
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
        public string? PhotoBase64 { get; set; }
        public bool IsActive { get; set; }

    }
}
