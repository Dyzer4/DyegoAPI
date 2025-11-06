namespace TIVIT.CIPA.Api.Domain.Model
{
    public class UserAuth
    {
        public int Id { get;  set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public byte[] Password { get; set; }
        public bool IsActive { get; set; } = true;
        public bool FirstAccess { get; set; } = true;

        public DateOnly? LastLogin { get; set; }
        public DateTimeOffset? RefreshTokenExp { get; set; }
        public string PasswordRecoverKey { get; set; }
        public DateTimeOffset? PasswordRecoverKeyExp { get; set; }

        public User User { get; set; }

        public static UserAuth CreatePasswordRecover(int userId, string passwordRecoverKey, DateTimeOffset? passwordRecoverKeyExp)
        {
            return new()
            {
                UserId = userId,
                PasswordRecoverKey = passwordRecoverKey,
                PasswordRecoverKeyExp = passwordRecoverKeyExp
            };
        }

        public void UpdatePasswordRecover(string passwordRecoverKey, DateTimeOffset? passwordRecoverKeyExp)
        {
            PasswordRecoverKey = passwordRecoverKey;
            PasswordRecoverKeyExp = passwordRecoverKeyExp;
        }
    }
}
