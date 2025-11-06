namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Voter
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public int CompanyID { get; set; }
        public int ProfileId { get; set; }
        public string Registry { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string ContactEmail { get; set; }
        public string CorporatePhone { get; set; }
        public string ContactPhone { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }
        public string Token { get; set; }
        public bool HasVoted { get; set; }
        public string ExclusionReason { get; set; }
        public DateTime? ExclusionDate { get; set; }
        public string ExcludedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public Candidate Candidate { get; set; }
        public Election Election{ get; set; }

        public ICollection<VoterAction> VoterActions { get; set; } = new List<VoterAction>();

    }
}
