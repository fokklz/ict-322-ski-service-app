using Newtonsoft.Json;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Models;

namespace SkiServiceApp.Services
{
    public class StorageService : IStorageService
    {
        public List<StoredUserCredentials> StoredUsers => _storedUsers;
        private List<StoredUserCredentials> _storedUsers;

        public string? Token => _token;
        private string? _token;

        public StorageService()
        {
            SyncAsync().ConfigureAwait(true);
        }

        public void SetToken(string token)
        {
            _token = token;
        }

        public void StoreUser(string username, string token, string refreshToken)
        {
            _storedUsers.Add(new StoredUserCredentials()
            {
                Username = username,
                Token = token,
                RefreshToken = refreshToken
            });

            if (_storedUsers.Count > 3)
            {
                _storedUsers.RemoveAt(0);
            }
        }

        public async Task SaveChangesAsync()
        {
            await SecureStorage.SetAsync("users", JsonConvert.SerializeObject(_storedUsers));
            if (_token != null) await SecureStorage.SetAsync("token", _token);
        }

        private async Task SyncAsync()
        {
            var rawUsers = await SecureStorage.GetAsync("users") ?? "[]";
            _storedUsers = JsonConvert.DeserializeObject<List<StoredUserCredentials>>(rawUsers) ?? new List<StoredUserCredentials>();
            _token = await SecureStorage.GetAsync("token") ?? null;
        }
    }
}
