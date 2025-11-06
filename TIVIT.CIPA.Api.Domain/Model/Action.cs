using System.Reflection;

namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Action
    {
        public int Id { get; set; }
        public int? ModuleId { get; set; } 
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Module? Module { get; set; }

        public ICollection<VoterAction> VoterActions { get; set; } = new List<VoterAction>();

    }
}