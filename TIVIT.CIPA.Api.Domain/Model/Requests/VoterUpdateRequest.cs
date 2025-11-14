namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class VoterUpdateRequest
    {
        public int ElectionId { get; set; }
        public int? SiteId { get; set; }
        public int? ProfileId { get; set; }
        public string Registry { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string ContactEmail { get; set; }
        public string CorporatePhone { get; set; }
        public string ContactPhone { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }
    }
}