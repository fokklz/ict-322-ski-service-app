using CommunityToolkit.Maui.Views;
using SkiServiceApp.Common;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using SkiServiceApp.Views;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.ViewModels
{
    public class UserListViewModel : BaseViewModel
    {
        public ObservableCollection<OrderResponse> Items { get; private set; } = new ObservableCollection<OrderResponse>();

        public ICommand EditCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand StatusCommand { get; private set; }

        public UserListViewModel()
        {
            EditCommand = new Command<OrderResponse>(NavigateToDetailAsync);
            //CancelCommand = new Command<OrderResponse>(Change);

            LoadData();
        }

        private void LoadData()
        {
            // Data
        }

        private async void NavigateToDetailAsync(OrderResponse service)
        {
            if (service == null)
            {
                Debug.WriteLine("Service is null, cannot navigate to detail.");
                return;
            }

            // Navigieren Sie zur Detailansicht und übergeben Sie die ServiceId als Query-Parameter.
            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?ServiceId={service.Id}");
        }


        private void Change(OrderResponse service)
        {
            // Change Command
        }
    }
}
