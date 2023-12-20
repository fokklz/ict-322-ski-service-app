using SkiServiceApp.Common;
using SkiServiceApp.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using SkiServiceApp.Resources.Helper;

namespace SkiServiceApp.ViewModels
{
    public class ServiceDetailViewModel : BaseViewModel
    {
        private ServiceDataModel _service;

        public ServiceDataModel Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand StatusCommand { get; private set; }

        public ServiceDetailViewModel()
        {
            CancelCommand = new Command(ExecuteCancelCommand);
            EditCommand = new Command(ExecuteEditCommand);
            StatusCommand = new Command(ExecuteStatusCommand);
        }

        public void LoadServiceDetails(int serviceId)
        {
            var testData = new ServiceDataModel
            {
                Id = serviceId,
                Priority = "Standard",
                Service = "Grosser Service",
                RemainingDays = "5",
                isAssigned = true,
                CustomerName = "Max Jupiter",
                Email = "max.jupiter@example.com",
                PhoneNumber = "0761726172",
                SubmissionDate = new DateTime(2023, 12, 11)
            };

            Service = testData;
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
