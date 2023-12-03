using SkiServiceApp.Models;

namespace SkiServiceApp.Interfaces
{
    public interface IStorageService
    {
        List<StoredUserCredentials> StoredUsers { get; }
        string? Token { get; }

        Task SaveChangesAsync();
        void SetToken(string token);
        void StoreUser(string username, string token, string refreshToken);
    }
}