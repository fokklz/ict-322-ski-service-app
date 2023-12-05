namespace SkiServiceApp.Models
{
    /// <summary>
    /// Used to store user credentials in a secure local storage
    /// Allows to login automatically (on click)
    /// </summary>
    public class StoredUserCredentials
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
