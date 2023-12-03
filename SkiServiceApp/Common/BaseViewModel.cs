namespace SkiServiceApp.Common
{
    public class BaseViewModel //: INotifyPropertyChanged
    {
        /*public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T backingStore, T value, string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));*/

        protected T GetService<T>()
            where T : class
        {
            var success = false;
            T? target;

            try
            {
                var handler = (Application.Current as App)?.MainPage?.Handler;
                var services = handler?.MauiContext?.Services;
                target = services?.GetService(typeof(T)) as T;
                // Fallback approach?
                //if (target == null)
                //{
                //    target = Shell.Current?.CurrentPage.Handler?.MauiContext?.Services.GetService(typeof(T)) as T;
                //}
                success = true;
            }
            catch (Exception)
            {
                target = null;
                success = false;
            }

            if (target == null || !success)
            {
                throw new Exception($"Service of type {typeof(T)} not found");
            }

            return target;
        }
    }
}
