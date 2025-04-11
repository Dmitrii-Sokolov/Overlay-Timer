using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Overlay_Timer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      InitializeTimer();
      CreateTrayIcon();

      UpdateTimer();

      var timer = new DispatcherTimer();
      timer.Interval = TimeSpan.FromSeconds(1);
      timer.Tick += (s, e) => UpdateTimer();
      timer.Start();
    }

    private void InitializeTimer()
    {
    }

    private void CreateTrayIcon()
    {
      var menu = new ContextMenuStrip();
      menu.Items.Add("Open", null, (s, e) => ShowMainWindow());
      menu.Items.Add("Hide", null, (s, e) => HideMainWindow());
      menu.Items.Add("Exit", null, (s, e) => Close());

      var trayIcon = new NotifyIcon();
      trayIcon.Text = Title;
      trayIcon.Visible = true;
      trayIcon.MouseClick += (s, e) => OnMouseClick(e);
      trayIcon.ContextMenuStrip = menu;
      trayIcon.Icon = IconHelper.DrawingImageToIcon((DrawingImage)Icon, 256, 256);
    }

    private void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
        SwitchMainWindow();
    }

    private void SwitchMainWindow() => WindowState = WindowState == WindowState.Normal ? WindowState.Minimized : WindowState.Normal;

    private void ShowMainWindow() => WindowState = WindowState.Normal;

    private void HideMainWindow() => WindowState = WindowState.Minimized;

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);

      DragMove();
    }

    private void UpdateTimer()
    {
      UpdateClock();
      UpdateProgressBar();
    }

    private void UpdateClock()
    {
      TimerText.Text = DateTime.Now.ToString("HH:mm");
    }

    private void UpdateProgressBar()
    {
      var frac = DateTime.Now.Second / 60f;

      UpdateFillArc(frac);
    }

    private void UpdateFillArc(double fraction)
    {
      var center = new Point(100, 100);
      double radius = 80;

      var sweep = 360 * Math.Max(0, Math.Min(1, fraction));
      double startAngle = -90;
      var endAngle = startAngle + sweep;

      var p1 = PointOnCircle(center, radius, startAngle);
      var p2 = PointOnCircle(center, radius, endAngle);

      var largeArc = sweep > 180;

      var fig = new PathFigure { StartPoint = p1, IsClosed = false };
      fig.Segments.Add(new ArcSegment(
          p2,
          new Size(radius, radius),
          0,
          largeArc,
          SweepDirection.Clockwise,
          true));

      ArcFill.Data = new PathGeometry(new[] { fig });
    }

    private static Point PointOnCircle(Point center, double radius, double angleDegrees)
    {
      var rad = angleDegrees * Math.PI / 180.0;
      return new Point(
          center.X + (radius * Math.Cos(rad)),
          center.Y + (radius * Math.Sin(rad)));
    }
  }
}
