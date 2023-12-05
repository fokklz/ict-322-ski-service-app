using UIKit;

namespace SkiServiceApp.Platforms
{
    public class KeyboardHelper
    {
        public static void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}
