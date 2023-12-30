using Newtonsoft.Json;
using PropertyChanged;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SkiServiceApp.Services
{
    /// <summary>
    /// Mainly used to store past users and their tokens so they can login back quickly
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class StorageService : IStorageService
    {
        private const int MaxStoredUsers = 3;

        public ObservableCollection<StoredUserCredentials> StoredUsers { get; private set; }

        public ObservableCollection<StoredUserCredentials> ReversedUsers { get; private set; }

        public StorageService()
        {
            StoredUsers = new ObservableCollection<StoredUserCredentials>();
            ReversedUsers = new ObservableCollection<StoredUserCredentials>();

            StoredUsers.CollectionChanged += _updateReversedUsers;
        }

        /// <summary>
        /// Loads the stored data from the secure storage
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await LoadStoredDataAsync();
        }

        /// <summary>
        /// Stores the given user credentials in the secure storage
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="token">The token of the user</param>
        /// <param name="refreshToken">The refresh token of the user</param>
        public void StoreUser(string username, string token, string refreshToken)
        {
            StoredUsers.Where(u => u.Username == username).ToList().ForEach(u => StoredUsers.Remove(u));
            StoredUsers.Add(new StoredUserCredentials
            {
                Username = username,
                Token = token,
                RefreshToken = refreshToken
            });

            // remove oldest entry if max is reached
            while (StoredUsers.Count > MaxStoredUsers)
            {
                StoredUsers.RemoveAt(0);
            }
        }

        /// <summary>
        /// Remove a stored user by its refresh token (used to logout)
        /// </summary>
        /// <param name="refreshToken">The refresh token of the user to remove</param>
        public void RemoveUserByRefreshToken(string refreshToken)
        {
            StoredUsers.Where(u => u.RefreshToken == refreshToken).ToList().ForEach(u => StoredUsers.Remove(u));
        }

        /// <summary>
        /// Writes the stored data to the secure storage
        /// </summary>
        public async Task SaveChangesAsync()
        {
            try
            {
                await SecureStorage.SetAsync("users", JsonConvert.SerializeObject(StoredUsers.ToList()));
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine("AT SAVING", ex.Message);
            }
        }
        
        /// <summary>
        /// Reverse the stored users to have the newest on top
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _updateReversedUsers(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ReversedUsers.Clear();
            StoredUsers.ToArray().Reverse().ToList().ForEach(u => ReversedUsers.Add(u));
        }

        /// <summary>
        /// Loads the stored data from the secure storage
        /// </summary>
        private async Task LoadStoredDataAsync()
        {
            try
            {
                var rawUsers = await SecureStorage.GetAsync("users") ?? "[]";
                var users = JsonConvert.DeserializeObject<List<StoredUserCredentials>>(rawUsers);
                // nothing to update
                if (users == null) return;

                StoredUsers.Clear();
                users.ForEach(u => StoredUsers.Add(u));
            }
            catch (Exception ex)
            {
                // Handle exceptions (should not happen?)
                Debug.WriteLine("AT LOADING", ex.Message);
            }
        }
    }
}
