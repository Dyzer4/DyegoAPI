namespace TIVIT.CIPA.Api.Domain.Model
{
    public class VoterAction
    {
        public int VoterId { get; set; }
        public int ActionId { get; set; }

        public Voter Voter { get; set; }
        public Action Action { get; set; }
    }
}