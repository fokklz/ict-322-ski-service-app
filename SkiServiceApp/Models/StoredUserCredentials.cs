namespace SkiServiceApp.Models
{
    public class StoredUserCredentials
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
