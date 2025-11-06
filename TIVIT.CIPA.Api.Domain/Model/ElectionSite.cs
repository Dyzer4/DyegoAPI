namespace TIVIT.CIPA.Api.Domain.Model
{
    public class ElectionSite
    {
        public int ElectionId { get; set; }
        public int SiteId { get; set; }

        public Election Election { get; set; }
        public Site Site { get; set; }
    }
}
