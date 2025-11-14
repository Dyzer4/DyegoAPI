namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Site
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string ProtheusCode { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public ICollection<Voter> Voters { get; set; }
        public virtual ICollection<ElectionSite> ElectionSites { get; set; }
    }
}
