namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public class UserFirstAccessEmailRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string TempPassword { get; set; }

        public UserFirstAccessEmailRequest(string fullName, string email, string tempPassword)
        {
            FullName = fullName;
            Email = email;
            TempPassword = tempPassword;
        }
    }
}