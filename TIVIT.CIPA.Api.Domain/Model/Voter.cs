namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Voter
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public int SiteId { get; set; }
        public int ProfileId { get; set; }
        public string CorporateId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string CorporatePhone { get; set; }
        public string ContactPhone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Area { get; set; }

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

        public Site Site { get; set; }
        public Election Election { get; set; }
        public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
        public ICollection<VoterAction> VoterActions { get; set; } = new List<VoterAction>();
    }
}