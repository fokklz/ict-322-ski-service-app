using SkiServiceApp.Models;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public partial class OrderDetailPage : ContentPage
    {
        private int _orderId;

        public int OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;
                    OnPropertyChanged(nameof(OrderId));
                    var vm = BindingContext as OrderDetailViewModel;
                    vm?.LoadServiceDetails(_orderId);
                }
            }
        }

        public OrderDetailPage(OrderDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}
