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
        private ServiceDataModel _serviceData;

        public ServiceDataModel ServiceData
        {
            get => _serviceData;
            set => SetProperty(ref _serviceData, value);
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

        public void LoadServiceDetails(ServiceDataModel serviceData)
        {
            ServiceData = serviceData;
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
