using StageManager.Native.PInvoke;
using StageManager.Native.Window;

namespace StageManager.Strategies
{
    internal class NormalizeAndMinimizeWindowStrategy : IWindowStrategy
	{
		public void Show(IWindow window)
		{
			// Restacking a visible window must not normalize or replay its restore animation.
			if (Win32.IsWindowVisible(window.Handle) && !window.IsMinimized) return;
			var command = window.IsMinimized ? Win32.SW.SW_RESTORE
				: window.IsMaximized ? Win32.SW.SW_SHOWMAXIMIZED : Win32.SW.SW_SHOWNOACTIVATE;
			Win32.ShowWindowAsync(window.Handle, command);
		}

		public void Hide(IWindow window)
		{
			Win32.ShowWindowAsync(window.Handle, Win32.SW.SW_MINIMIZE);
		}
	}
}
