namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Profile
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Voter> Voters { get; set; }
        public ICollection<ProfileAction> ProfileActions { get; set; }

        public bool IsGlobalAdmin => Code.Equals(GlobalAdmin);
        public const string GlobalAdmin = "global_admin";
    }
}
