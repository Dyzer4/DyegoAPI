namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class UserUpdateRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CorporateId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int ProfileId { get; set; }
    }
}
