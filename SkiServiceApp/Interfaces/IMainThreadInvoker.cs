namespace SkiServiceApp.Interfaces
{
    public interface IMainThreadInvoker
    {
        void BeginInvokeOnMainThread(Action action);
    }
}
