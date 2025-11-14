namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class CandidateElectionListResponse
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string PhotoBase64 { get; set; }
    }
}