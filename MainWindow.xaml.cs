using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace OverlayTimer
{
  public partial class MainWindow : Window
  {
    private DateTime PeriodStart;
    private DateTime PeriodEnd;
    private DateTime AlarmEnd;
    private TimeSpan PeriodDuration;

    //TODO Add Alarm
    //TODO Add period tuning
    //TODO Add weekday tuning
    //TODO Add click transparency
    //TODO Add GitHub build

    public MainWindow()
    {
      InitializeComponent();

      InitializeTime();

      CreateTrayIcon();

      SizeChanged += (s, e) => UpdateTimer();

      Loaded += (s, e) => ScheduleRenderUpdates();
    }

    private void InitializeTime()
    {
      PeriodStart = DateTime.Today.AddHours(8).AddMinutes(40);
      PeriodEnd = DateTime.Today.AddHours(15).AddMinutes(40);
      AlarmEnd = PeriodEnd.AddMinutes(20);

      PeriodDuration = PeriodEnd - PeriodStart;
    }

    private void CreateTrayIcon() => new TrayIcon(this);

    private void ScheduleRenderUpdates()
    {
      var timer = new DispatcherTimer();
      timer.Interval = TimeSpan.FromSeconds(1);
      timer.Tick += (s, e) => UpdateTimer();
      timer.Start();
    }

    private void UpdateTimer()
    {
      var remainingTime = PeriodEnd - DateTime.Now;

      UpdateClock(remainingTime);
      UpdateProgressBar(remainingTime);
    }

    private void UpdateClock(TimeSpan remainingTime)
    {
      TimerText.Text = remainingTime.ToString(@"hh\:mm");
    }

    private void UpdateProgressBar(TimeSpan remainingTime)
    {
      var fraction = Math.Max(remainingTime.TotalMilliseconds / PeriodDuration.TotalMilliseconds, 0);
      UpdateFillArc(fraction);
    }

    private void UpdateFillArc(double fraction)
    {
      var center = new Point(0.5d * Content.ActualWidth, 0.5d * Content.ActualHeight);
      var halfThickness = 0.5d * ArcBack.StrokeThickness;
      var halfWidth = 0.5d * Content.ActualWidth - halfThickness;
      var halfHeight = 0.5d * Content.ActualHeight - halfThickness;

      var sweep = 2d * Math.PI * fraction;
      var startAngle = -0.5d * Math.PI;
      var endAngle = startAngle + sweep;

      var point0 = PointOnCircle(center, halfWidth, halfHeight, startAngle);
      var point1 = PointOnCircle(center, halfWidth, halfHeight, endAngle);

      var largeArc = sweep > Math.PI;

      var figure = new PathFigure { StartPoint = point0, IsClosed = false };
      figure.Segments.Add(new ArcSegment(
          point1,
          new Size(halfWidth, halfHeight),
          0,
          largeArc,
          SweepDirection.Clockwise,
          true));

      ArcFill.Data = new PathGeometry(new[] { figure });
    }

    private static Point PointOnCircle(Point center, double halfWidth, double halfHeight, double angle)
      => new Point(
        center.X + halfWidth * Math.Cos(angle),
        center.Y + halfHeight * Math.Sin(angle));
  }
}
