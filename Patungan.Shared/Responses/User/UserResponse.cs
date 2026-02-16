namespace Patungan.Shared.Responses.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
