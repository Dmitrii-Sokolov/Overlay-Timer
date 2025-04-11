using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace OverlayTimer
{
  public class TrayIcon
  {
    private bool Visible = true;

    public Window Window { get; }

    public TrayIcon(Window window)
    {
      Window = window;

      var menu = new ContextMenuStrip();
      menu.Items.Add("Show", null, (s, e) => ShowWindow());
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

    private void SwitchWindow()
    {
      if (Visible)
      {
        HideWindow();
      }
      else
      {
        ShowWindow();
      }
    }

    private void ShowWindow()
    {
      Visible = true;
      Window.Show();
    }

    private void HideWindow()
    {
      Visible = false;
      Window.Hide();
    }
  }
}
