namespace TIVIT.CIPA.Api.Domain.Interfaces.Models
{
    public interface IUserInfo
    {
        string Name { get; }
        string UniqueName { get; }
        public string Upn { get; }
        public string AppId { get; }
        public string Role { get; }
        public string Language { get; }
    }
}
