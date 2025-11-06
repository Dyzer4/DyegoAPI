namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class ElectionCreateRequest
    {
        public string Name { get; set; }
        public DateTime ElectionStartDate { get; set; }
        public DateTime ElectionEndDate { get; set; }
        public List<int> SitesIds { get; set; }
    }
}
