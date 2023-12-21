using SkiServiceApp.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        public ObservableCollection<OrderResponse> Items { get; private set; } = new ObservableCollection<OrderResponse>();

        public ICommand AssignCommand { get; private set; }

        public ListViewModel()
        {
            AssignCommand = new Command<int>(AssignItem);
            LoadData();
        }

        private void LoadData()
        {
            // Data
        }

        private void AssignItem(int id)
        {
            Debug.WriteLine($"Item mit ID {id} zugewiesen.");
        }
    }
}
