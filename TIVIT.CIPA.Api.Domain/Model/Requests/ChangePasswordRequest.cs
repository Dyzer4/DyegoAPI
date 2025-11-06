namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public record ChangePasswordRequest(string Key, string Password, string ConfirmPassword);
    public record FirstAccessPasswordRequest(string Password, string ConfirmPassword);
}
