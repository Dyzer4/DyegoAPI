namespace TIVIT.CIPA.Api.Domain.Model
{
    public class ExclusionLog
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public int VoterId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Reason { get; set; }
        public string PerformedBy { get; set; }

        public virtual Election Election { get; set; }
        public virtual Voter Voter { get; set; }
    }
}
