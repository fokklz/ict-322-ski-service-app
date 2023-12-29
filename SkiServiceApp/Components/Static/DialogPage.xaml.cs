using PropertyChanged;

namespace SkiServiceApp.Views;

[AddINotifyPropertyChangedInterface]
public partial class DialogPage : ContentPage
{
    /// <summary>
    /// Actual content of the dialog
    /// Can contain any view (PageView)
    /// </summary>
    public ContentView CustomContent { get; set; }

    /// <summary>
    /// The text that will be displayed on the title of the dialog
    /// </summary>
    public string TitleText { get; set; }

    /// <summary>
    /// The Command that will be executed when the dialog is closed (using cancel)
    /// </summary>
    public Command CloseCommand { get; set; }

    /// <summary>
    /// The text that will be displayed on the close button
    /// </summary>
    public string CloseText { get; set; }

    /// <summary>
    /// The Command that will be executed when the dialog is closed (using submit)
    /// </summary>
    public Command SubmitCommand { get; set; }

    /// <summary>
    /// The text that will be displayed on the submit button
    /// </summary>
    public string SubmitText { get; set; }

    /// <summary>
    /// The text that will be displayed on the danger button
    /// </summary>
    [AlsoNotifyFor(nameof(IsUsingDanger))]
    public string? DangerText { get; set; } = null;

    /// <summary>
    /// The Command that will be executed when the dialog is closed (using danger)
    /// </summary>
    [AlsoNotifyFor(nameof(IsUsingDanger))]
    public Command? DangerCommand { get; set; } = null;

    /// <summary>
    /// Whether the dialog is using a danger button
    /// </summary>
    public bool IsUsingDanger => !string.IsNullOrEmpty(DangerText) && DangerCommand != null;

    public DialogPage()
    {
        InitializeComponent();
        BindingContext = this;

        Prepare();
    }

    /// <summary>
    /// Sets the dialog in a closed state so it can be animated in
    /// </summary>
    public void Prepare()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DialogBackground.Opacity = 0;
            DialogContent.Opacity = 0;
            DialogContent.Scale = 0.7f;
        });
    }

    /// <summary>
    /// Animates the dialog in
    /// </summary>
    public void OpenAnimation()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.WhenAll(
                DialogContent.FadeTo(1, 200, Easing.SinIn),
                DialogContent.ScaleTo(1, 200, Easing.SinIn),
                DialogBackground.FadeTo(1, 300, Easing.SinIn)
            );
        });
    }

    /// <summary>
    /// Animates the dialog out
    /// </summary>
    public void CloseAnimation()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.WhenAll(
                DialogContent.ScaleTo(0.7f, 200, Easing.SinIn),
                DialogContent.FadeTo(0, 200, Easing.SinIn),
                DialogBackground.FadeTo(0, 300, Easing.SinIn)
            );
        });
    }
}