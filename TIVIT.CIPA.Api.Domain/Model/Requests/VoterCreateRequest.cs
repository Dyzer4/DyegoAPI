namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class VoterCreateRequest
    {
        public int ElectionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
