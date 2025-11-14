namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Candidate
    {
        public int Id { get; set; }
        public int VoterId { get; set; }
        public byte[] PhotoBase64 { get; set; }
        public string PhotoMimeType { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsResultRelease { get; set; } = false;

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public Voter Voter { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}