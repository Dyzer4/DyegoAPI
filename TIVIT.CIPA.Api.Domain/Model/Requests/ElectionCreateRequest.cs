namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class ElectionCreateRequest
    {
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? ElectionStartDate { get; set; }
        public DateTime? ElectionEndDate { get; set; }
        public string Type { get; set; }
        public string InvitationMessage { get; set; }
        public List<int> SitesIds { get; set; }
    }
}