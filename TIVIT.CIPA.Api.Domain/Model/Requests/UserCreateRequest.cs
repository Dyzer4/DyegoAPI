namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class UserCreateRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CorporateId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Status { get; set; }
        //public DateTime? AdmissionDate { get; set; }
        public string NetworkUser { get; set; }
        public int ProfileId { get; set; }
    }
}
