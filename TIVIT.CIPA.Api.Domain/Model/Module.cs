namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Action> Actions { get; set; }
    }
}