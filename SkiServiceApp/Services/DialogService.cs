using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Views;

namespace SkiServiceApp.Services
{
    public class DialogService
    {
        public static async Task ShowDialog(IDialog dialog, Action<bool> onDialogClosed, string? submitText = null, string? titleText = null, string? closeText = null, string? dangerText = null, Command? dangerCommand = null)
        {
            var mainPage = Application.Current.MainPage;


            // Wrap the dialog in a CustomDialogPage
            var dialogPage = new DialogPage()
            {
                CustomContent = dialog as ContentView,
                CloseText = closeText ?? Localization.Instance.Dialog_Cancel,
                SubmitText = submitText ?? Localization.Instance.Dialog_Submit,
                TitleText = titleText ?? Localization.Instance.Dialog_Title,
            };

            var commonCloseCommand = new Command(async () =>
            {
                dialogPage.CloseAnimation();
                await mainPage.Navigation.PopModalAsync(false);
            });

            if (!string.IsNullOrEmpty(dangerText))
            {
                dialogPage.DangerText = dangerText;
                dialogPage.DangerCommand = new Command(() =>
                {
                    commonCloseCommand.Execute(null);
                    if (dangerCommand != null && dangerCommand.CanExecute(null))
                        dangerCommand.Execute(null);
                    onDialogClosed(true);
                });
            }

            dialogPage.SubmitCommand = new Command(() => {
                commonCloseCommand.Execute(null);
                onDialogClosed(true);
            });
            dialogPage.CloseCommand = new Command(() =>
            {
                commonCloseCommand.Execute(null);
                onDialogClosed(false);
            });

            await mainPage.Navigation.PushModalAsync(dialogPage, false);
            dialogPage.OpenAnimation();
        }
    }
}
