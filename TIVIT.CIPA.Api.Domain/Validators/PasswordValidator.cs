using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.Extensions.Localization;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Validators
{
    public class PasswordValidator : Notifiable<Notification>
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        private const int MinLength = 8;
        private const int MaxLength = 16;

        public PasswordValidator(
            IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public void Validate(ChangePasswordRequest request)
        {
            AddNotifications(new Contract<ChangePasswordRequest>()
                .IsNotNullOrWhiteSpace(request.Password, "InvalidPassword", _localizer[AuthMessages.InvalidPassword])
                .IsTrue(MatchPassword(request.Password, request.ConfirmPassword), "NotMatchPassword", _localizer[AuthMessages.NotMatchPassword])
                .IsTrue(ValidPasswordLength(request.Password), "PasswordMustBeBetweenCharacters", string.Format(_localizer[AuthMessages.PasswordMustBeBetweenCharacters], MinLength, MaxLength))
                .IsTrue(HasLowerCase(request.Password), "PasswordMustBeBetweenCharacters", _localizer[AuthMessages.PasswordMustHaveAtLeastLowercaseLetter])
                .IsTrue(HasUpperCase(request.Password), "PasswordMustHaveAtLeastUppercaseLetter", _localizer[AuthMessages.PasswordMustHaveAtLeastUppercaseLetter])
                .IsTrue(HasDigit(request.Password), "PasswordMustHaveAtLeastDigit", _localizer[AuthMessages.PasswordMustHaveAtLeastDigit])
                .IsTrue(HasSpecialCharacters(request.Password), "PasswordMustHaveAtLeastSpecialCharacter", _localizer[AuthMessages.PasswordMustHaveAtLeastSpecialCharacter])
            );
        }

        public void Validate(FirstAccessPasswordRequest request)
        {
            AddNotifications(new Contract<ChangePasswordRequest>()
                .IsNotNullOrWhiteSpace(request.Password, "InvalidPassword", _localizer[AuthMessages.InvalidPassword])
                .IsTrue(MatchPassword(request.Password, request.ConfirmPassword), "NotMatchPassword", _localizer[AuthMessages.NotMatchPassword])
                .IsTrue(ValidPasswordLength(request.Password), "PasswordMustBeBetweenCharacters", string.Format(_localizer[AuthMessages.PasswordMustBeBetweenCharacters], MinLength, MaxLength))
                .IsTrue(HasLowerCase(request.Password), "PasswordMustBeBetweenCharacters", _localizer[AuthMessages.PasswordMustHaveAtLeastLowercaseLetter])
                .IsTrue(HasUpperCase(request.Password), "PasswordMustHaveAtLeastUppercaseLetter", _localizer[AuthMessages.PasswordMustHaveAtLeastUppercaseLetter])
                .IsTrue(HasDigit(request.Password), "PasswordMustHaveAtLeastDigit", _localizer[AuthMessages.PasswordMustHaveAtLeastDigit])
                .IsTrue(HasSpecialCharacters(request.Password), "PasswordMustHaveAtLeastSpecialCharacter", _localizer[AuthMessages.PasswordMustHaveAtLeastSpecialCharacter])
            );
        }

        static bool MatchPassword(string password, string confirmPassword) => password == confirmPassword;
        static bool ValidPasswordLength(string password) => password.Length >= MinLength && password.Length <= MaxLength;
        static bool HasLowerCase(string password) => password.Any(char.IsLower);
        static bool HasUpperCase(string password) => password.Any(char.IsUpper);
        static bool HasDigit(string password) => password.Any(char.IsDigit);
        static bool HasSpecialCharacters(string password) => !password.All(char.IsLetterOrDigit);
    }
}
