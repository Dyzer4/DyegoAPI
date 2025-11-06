namespace TIVIT.CIPA.Api.Domain.Model
{
    public class ProfileAction
    {
        public int ProfileId { get; set; }
        public int ActionId { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsChecked { get; set; }

        public Profile Profile { get; set; }
        public Action Action { get; set; }
    }
}
