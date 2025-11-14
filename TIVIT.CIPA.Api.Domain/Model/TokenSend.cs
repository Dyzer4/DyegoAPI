namespace TIVIT.CIPA.Api.Domain.Model
{
    public class TokenSend
    {
        public int Id { get; set; }
        public int VoterId { get; set; }
        public string Token { get; set; }
        public DateTime SentAt { get; set; }
        public string SendType { get; set; }
        public bool IsActive { get; set; }

        public virtual Voter Voter { get; set; }
    }
}
