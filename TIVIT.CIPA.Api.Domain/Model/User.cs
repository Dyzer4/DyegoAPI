namespace TIVIT.CIPA.Api.Domain.Model
{
    public class User
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CorporateId { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public Profile Profile { get; set; }
    }
}
