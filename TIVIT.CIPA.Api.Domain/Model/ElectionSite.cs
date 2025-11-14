namespace TIVIT.CIPA.Api.Domain.Model
{
    public class ElectionSite
    {
        public int ElectionId { get; set; }
        public int SiteId { get; set; }

        public virtual Election Election { get; set; }
        public virtual Site Site { get; set; }
    }
}
