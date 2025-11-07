namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class VoterListResponse
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public string CorporateId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string? SiteName { get; set; }
    }
}