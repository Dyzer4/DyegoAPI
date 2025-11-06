namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class SiteResponse
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string ProtheusCode { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
