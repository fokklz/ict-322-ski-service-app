using SkiServiceApp.Models;
using System.Collections.ObjectModel;

namespace SkiServiceApp.Interfaces
{
    public interface IStorageService
    {
        ObservableCollection<StoredUserCredentials> StoredUsers { get; }
        ObservableCollection<StoredUserCredentials> ReversedUsers { get; }

        Task InitializeAsync();
        Task SaveChangesAsync();
        void StoreUser(string username, string token, string refreshToken);
        void RemoveUserByRefreshToken(string refreshToken);
    }
}