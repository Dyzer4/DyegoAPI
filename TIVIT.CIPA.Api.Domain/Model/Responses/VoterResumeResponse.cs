
namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class VoterResumeResponse
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public string Registry { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public bool HasVoted { get; set; }
    }
}
