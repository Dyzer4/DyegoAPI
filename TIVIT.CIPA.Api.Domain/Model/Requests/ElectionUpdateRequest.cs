namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class ElectionUpdateRequest
    {
        public string Name { get; set; }
        public DateTime ElectionStartDate { get; set; }
        public DateTime ElectionEndDate { get; set; }
        public List<int> SitesIds { get; set; }
    }
}
