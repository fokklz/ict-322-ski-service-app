using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace SkiServiceApp.Components
{
    public partial class CustomListItem : ContentView
    {
        public static readonly BindableProperty PriorityServiceProperty =
            BindableProperty.Create(nameof(PriorityService), typeof(string), typeof(CustomListItem));

        public static readonly BindableProperty RemainingDaysAssignmentProperty =
            BindableProperty.Create(nameof(RemainingDaysAssignment), typeof(string), typeof(CustomListItem));

        public static readonly BindableProperty AssignCommandProperty =
            BindableProperty.Create(nameof(AssignCommand), typeof(ICommand), typeof(CustomListItem));

        public static readonly BindableProperty IdProperty =
            BindableProperty.Create(nameof(Id), typeof(int), typeof(CustomListItem));

        public string PriorityService
        {
            get => (string)GetValue(PriorityServiceProperty);
            set => SetValue(PriorityServiceProperty, value);
        }

        public string RemainingDaysAssignment
        {
            get => (string)GetValue(RemainingDaysAssignmentProperty);
            set => SetValue(RemainingDaysAssignmentProperty, value);
        }

        public ICommand AssignCommand
        {
            get => (ICommand)GetValue(AssignCommandProperty);
            set => SetValue(AssignCommandProperty, value);
        }

        public int Id
        {
            get => (int)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public CustomListItem()
        {
            InitializeComponent();
        }
    }
}
