using CommunityToolkit.Maui.Views;
using SkiServiceApp.Common;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using SkiServiceApp.Views;

namespace SkiServiceApp.ViewModels
{
    public class UserListViewModel : BaseViewModel
    {
        public ObservableCollection<ServiceDataModel> Items { get; private set; } = new ObservableCollection<ServiceDataModel>();

        public ICommand EditCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand StatusCommand { get; private set; }

        public UserListViewModel()
        {
            EditCommand = new Command<ServiceDataModel>(NavigateToDetailAsync);
            CancelCommand = new Command<ServiceDataModel>(Change);

            LoadData();
        }

        private void LoadData()
        {
            Items.Add(new ServiceDataModel
            {
                Id = 1,
                Priority = "Standard",
                Service = "Grosser Service",
                RemainingDays = "5",
                isAssigned = true,
                CustomerName = "Max Jupiter",
                Email = "max.jupiter@example.com",
                PhoneNumber = "0761726172",
                SubmissionDate = new DateTime(2023, 12, 11)
            });
            Items.Add(new ServiceDataModel
            {
                Id = 2,
                Priority = "Standard",
                Service = "Kleiner Service",
                Status = "In Bearbeitung",
                RemainingDays = "3",
                isAssigned = true,
                CustomerName = "Max Mustermann",
                Email = "max.mustermann@example.com",
                PhoneNumber = "0123456789",
                SubmissionDate = new DateTime(2023, 12, 11)
            });
            Items.Add(new ServiceDataModel
            {
                Id = 4,
                Priority = "Hoch",
                Service = "Kleiner Service",
                Status = "Offen",
                RemainingDays = "1",
                isAssigned = true,
                CustomerName = "Moritz Müller",
                Email = "moritz.mueller@example.com",
                PhoneNumber = "0617267162",
                SubmissionDate = new DateTime(2023, 12, 19)
            });
        }

        private async void NavigateToDetailAsync(ServiceDataModel service)
        {
            if (service == null)
            {
                Debug.WriteLine("Service is null, cannot navigate to detail.");
                return;
            }

            // Navigieren Sie zur Detailansicht und übergeben Sie das ServiceDataModel-Objekt.
            var navigationParameter = new Dictionary<string, object>
            {
                { "Service", service }
            };
            await Shell.Current.GoToAsync(nameof(ServiceDetailView), navigationParameter);
        }


        private void Change(ServiceDataModel service)
        {
            // Change Command
        }
    }
}
