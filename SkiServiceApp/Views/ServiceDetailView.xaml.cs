using SkiServiceApp.Models;
using SkiServiceApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Views
{
    [QueryProperty(nameof(Service), "Service")]
    public partial class ServiceDetailView : ContentPage
    {
        public ServiceDetailViewModel ViewModel => BindingContext as ServiceDetailViewModel;

        public ServiceDetailView()
        {
            InitializeComponent();
            BindingContext = new ServiceDetailViewModel();
        }

        public ServiceDataModel Service
        {
            set
            {
                ViewModel?.LoadServiceDetails(value);
            }
        }
    }

}
