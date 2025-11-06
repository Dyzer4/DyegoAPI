namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class UserDetailResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CorporateId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ProfileId { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<int> Actions { get; set; } 
    }
}
