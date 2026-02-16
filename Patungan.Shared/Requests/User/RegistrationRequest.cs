namespace Patungan.Shared.Requests.User
{
    public class RegistrationRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
    }
}
