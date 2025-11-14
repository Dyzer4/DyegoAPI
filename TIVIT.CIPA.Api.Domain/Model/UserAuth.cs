namespace TIVIT.CIPA.Api.Domain.Model
{
    public class UserAuth
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte[] Password { get; set; }  // ✅ MUDADO: string → byte[]
        public string RefreshToken { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public string PasswordRecoverKey { get; set; }
        public DateTime? PasswordRecoverKeyExp { get; set; }
        public bool FirstAccess { get; set; }

        public virtual User User { get; set; }

        public static UserAuth CreatePasswordRecover(int userId, string passwordRecoverKey, DateTime? passwordRecoverKeyExp)
        {
            return new()
            {
                UserId = userId,
                PasswordRecoverKey = passwordRecoverKey,
                PasswordRecoverKeyExp = passwordRecoverKeyExp,
                IsActive = true
            };
        }

        internal static UserAuth CreatePasswordRecover(int id, string key, DateTimeOffset expiresUtc)
        {
            throw new NotImplementedException();
        }

        public void UpdatePasswordRecover(string passwordRecoverKey, DateTime? passwordRecoverKeyExp)
        {
            PasswordRecoverKey = passwordRecoverKey;
            PasswordRecoverKeyExp = passwordRecoverKeyExp;
        }

        internal void UpdatePasswordRecover(string key, DateTimeOffset expiresUtc)
        {
            throw new NotImplementedException();
        }
    }
}
