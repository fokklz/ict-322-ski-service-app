using SkiServiceApp.Interfaces;

namespace SkiServiceApp.Common.Helpers
{
    public class MainThreadInvoker : IMainThreadInvoker
    {
        public void BeginInvokeOnMainThread(Action action)
        {
            MainThread.BeginInvokeOnMainThread(action);
        }
    }
}
