using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace OverlayTimer
{
  public class TrayIcon
  {
    public Window Window { get; }

    public TrayIcon(Window window)
    {
      Window = window;

      var menu = new ContextMenuStrip();
      menu.Items.Add("Open", null, (s, e) => ShowWindow());
      menu.Items.Add("Hide", null, (s, e) => HideWindow());
      menu.Items.Add("Exit", null, (s, e) => Window.Close());

      var trayIcon = new NotifyIcon();
      trayIcon.Text = window.Title;
      trayIcon.Visible = true;
      trayIcon.MouseClick += (s, e) => OnMouseClick(e);
      trayIcon.ContextMenuStrip = menu;
      trayIcon.Icon = IconHelper.DrawingImageToIcon((DrawingImage)window.Icon, 256, 256);
    }

    private void OnMouseClick(MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
        SwitchWindow();
    }

    private void SwitchWindow() =>
      Window.WindowState = Window.WindowState == WindowState.Normal
      ? WindowState.Minimized
      : WindowState.Normal;

    private void ShowWindow() => Window.WindowState = WindowState.Normal;

    private void HideWindow() => Window.WindowState = WindowState.Minimized;
  }
}
