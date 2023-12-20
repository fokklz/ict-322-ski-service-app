using SkiServiceApp.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using SkiServiceApp.Models;

namespace SkiServiceApp.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        public ObservableCollection<ServiceDataModel> Items { get; private set; } = new ObservableCollection<ServiceDataModel>();

        public ICommand AssignCommand { get; private set; }

        public ListViewModel()
        {
            AssignCommand = new Command<int>(AssignItem);
            LoadData();
        }

        private void LoadData()
        {
            Items.Add(new ServiceDataModel
            {
                Id = 3,
                Priority = "Standard",
                Service = "Kleiner Service",
                RemainingDays = "3",
                isAssigned = false,
                CustomerName = "Max Mustermann",
                Email = "max.mustermann@example.com",
                PhoneNumber = "0123456789",
                SubmissionDate = new DateTime(2023, 12, 11)
            });
            Items.Add(new ServiceDataModel
            {
                Id = 5,
                Priority = "Hoch",
                Service = "Großer Service",
                RemainingDays = "5",
                isAssigned = false,
                CustomerName = "Erika Musterfrau",
                Email = "erika.musterfrau@example.com",
                PhoneNumber = "0987654321",
                SubmissionDate = new DateTime(2023, 12, 12)
            });
        }

        private void AssignItem(int id)
        {
            Debug.WriteLine($"Item mit ID {id} zugewiesen.");
        }
    }
}
