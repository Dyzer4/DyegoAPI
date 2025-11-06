using Jose;
using Microsoft.Extensions.Options;
using TIVIT.CIPA.Api.Domain.Settings;
using Encod = System.Text.Encoding;

namespace TIVIT.CIPA.Api.Domain.Providers
{
    public class AuthTokenProvider
    {
        private const int SecondsToExpire = 3600;

        private readonly AuthSettings _authSettings;

        public AuthTokenProvider(IOptionsMonitor<AuthSettings> authSettings)
        {
            _authSettings = authSettings.CurrentValue;
        }

        public string Create(Payload payload)
        {
            return JWT.Encode(payload, Encod.Unicode.GetBytes(_authSettings.SecTokenSecret), JwsAlgorithm.HS256);
        }

        internal Payload Decode(string token, out string errorMessage)
        {
            Payload payload = null;
            errorMessage = string.Empty;

            try
            {
                string json = JWT.Decode(token, Encod.Unicode.GetBytes(_authSettings.SecTokenSecret));
                payload = JWT.Payload<Payload>(token);
            }
            catch (Exception ex)
            {
                errorMessage = "Invalid authorization token.";
                Console.WriteLine(ex.Message);
            }

            return payload;
        }

        public static DateTimeOffset GetExpireDate() => DateTimeOffset.Now.AddSeconds(SecondsToExpire);

        public static double GetExp(DateTimeOffset expireDate) => expireDate.ToUnixTimeSeconds();

        private static bool ValidateRoles(string[] allowedRoles, string payloadRole) => allowedRoles.Contains(payloadRole);

        private static bool ValidateActions(string[] allowedActions, string[] payloadActions) => allowedActions.Intersect(payloadActions).Any();

        private static bool ValidateDate(DateTimeOffset expireDate) => (DateTime.Now - expireDate).TotalSeconds <= SecondsToExpire;

        private static bool ValidateUsername(string username, string payloadUsername) =>
            !string.IsNullOrWhiteSpace(username) && username.ToUpper() == payloadUsername?.ToUpper();

        public (bool isValid, Payload payload) ValidatePayload(string username, string token, string[] allowedRoles, string[] allowedActions,
            out string errorMessage)
        {
            var payload = Decode(token, out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage)) return (false, null);

            if (!ValidateUsername(username, payload.Sub))
            {
                errorMessage = "Invalid user.";
                return (false, null);
            }

            if (!ValidateDate(payload.ExpireDate))
            {
                errorMessage = "Token has expired.";
                return (false, null);
            }

            if (allowedRoles.Any() && !ValidateRoles(allowedRoles, payload.Role))
            {
                errorMessage = "The user does not have permission to the resource.";
                return (false, null);
            }

            if (allowedActions.Any() && !ValidateActions(allowedActions, payload.Actions))
            {
                errorMessage = "The user does not have permission to the resource.";
                return (false, null);
            }

            return (true, payload);
        }
    }

    public record Payload(string Sub, double Exp, DateTimeOffset ExpireDate, string Name, string Role, string[] Actions, int UserId, bool FirstAccess);
}
