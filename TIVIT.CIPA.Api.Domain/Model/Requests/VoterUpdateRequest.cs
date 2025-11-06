namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class VoterUpdateRequest
    {
        public int ElectionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

