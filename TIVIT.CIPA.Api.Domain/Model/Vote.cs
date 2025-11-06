using System.ClientModel;

namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Vote
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public int? CandidateId { get; set; }
        public byte[] EncryptedVote { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreateDate { get; set; }

        public Election Election { get; set; }
        public Candidate Candidate { get; set; }
    }
}
