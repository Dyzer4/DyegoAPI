namespace TIVIT.CIPA.Api.Domain.Model
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public int ActionId { get; set; }
    }
}
