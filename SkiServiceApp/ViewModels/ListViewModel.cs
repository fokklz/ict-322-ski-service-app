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
            // Test Daten
            Items.Add(new ServiceDataModel { Priority = "Standard", Service = "Kleiner Service", RemainingDays = "3", IsAssigned = "Nicht zugewiesen" });
        }

        private void AssignItem(int id)
        {
            Debug.WriteLine($"Item mit ID {id} zugewiesen.");
        }
    }
}
