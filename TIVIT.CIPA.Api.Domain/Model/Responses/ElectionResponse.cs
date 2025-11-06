namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class ElectionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ElectionStartDate { get; set; }
        public DateTime ElectionEndDate { get; set; }
        public bool IsActive { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public string UpdateUser { get; set; }
        public IEnumerable<SiteResponse> Sites { get; set; }
    }
}
