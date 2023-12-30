using SkiServiceApp.Interfaces;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;
using System.Windows.Input;

namespace SkiServiceApp.Components.Dialogs;

public partial class CancelDialog : ContentView, IDialog
{

	public ICommand SubmitCommand { get; set; }

	public CustomListItem Entry { get; set; }

	public CancelDialog(CustomListItem itemEntry)
	{
		InitializeComponent();

		Entry = itemEntry;
	}
}