using SkiServiceApp.Common;
using SkiServiceApp.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using SkiServiceApp.Resources.Helper;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel
    {
        private OrderResponse _service;

        public OrderResponse Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand StatusCommand { get; private set; }

        public OrderDetailViewModel()
        {
            CancelCommand = new Command(ExecuteCancelCommand);
            EditCommand = new Command(ExecuteEditCommand);
            StatusCommand = new Command(ExecuteStatusCommand);
        }

        public void LoadServiceDetails(int serviceId)
        {
            // Data
        }

        private void ExecuteCancelCommand()
        {
            // Storno
        }

        private void ExecuteEditCommand()
        {
            // Edit
        }

        private void ExecuteStatusCommand()
        {
            // Status
        }
    }
}
