namespace TIVIT.CIPA.Api.Domain.Model
{
    public class VoteLog
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public string VoteHash { get; set; }
        public DateTime Timestamp { get; set; }
        public string Protocol { get; set; }
        public string DeviceId { get; set; }
        public bool IsActive { get; set; }


        public virtual Election Election { get; set; }
    }
}
