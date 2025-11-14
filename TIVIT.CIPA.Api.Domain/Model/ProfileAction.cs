namespace TIVIT.CIPA.Api.Domain.Model
{
    public class ProfileAction
    {
        public int ProfileId { get; set; }
        public int ActionId { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsChecked { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Action Action { get; set; }
    }
}
