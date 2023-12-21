using SkiServiceApp.Models;
using SkiServiceApp.ViewModels;
using SkiServiceModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Views
{
    [QueryProperty(nameof(ServiceId), nameof(ServiceId))]
    public partial class OrderDetailPage : ContentPage
    {
        private int _serviceId;

        public int ServiceId
        {
            get => _serviceId;
            set
            {
                if (_serviceId != value)
                {
                    _serviceId = value;
                    OnPropertyChanged(nameof(ServiceId));
                    var vm = (OrderDetailViewModel)this.BindingContext;
                    vm?.LoadServiceDetails(_serviceId);
                }
            }
        }

        public OrderDetailPage()
        {
            InitializeComponent();
            BindingContext = new OrderDetailViewModel();
        }
    }

}
