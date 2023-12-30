namespace SkiServiceApp.Common.Events
{
    public class LoginChangedEventArgs : EventArgs
    {
        public bool IsLoggedIn { get; set; }

        public string? Token { get; set; }

        public int? UserId { get; set; }

        public LoginChangedEventArgs()
        {
            IsLoggedIn = false;
            Token = null;
            UserId = null;
        }

        public LoginChangedEventArgs(string? token, int? userId)
        {
            IsLoggedIn = true;
            Token = token;
            UserId = userId;
        }
    }
}
