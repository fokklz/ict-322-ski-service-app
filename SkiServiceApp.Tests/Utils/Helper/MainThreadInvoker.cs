using SkiServiceApp.Interfaces;

namespace SkiServiceApp.Tests.Util.Helper
{
    public class MainThreadInvoker : IMainThreadInvoker
    {
        public void BeginInvokeOnMainThread(Action action)
        {
            action.Invoke();
        }
    }
}
