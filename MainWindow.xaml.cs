using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Overlay_Timer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly DispatcherTimer Timer;

    public MainWindow()
    {
      InitializeComponent();

      Timer = new DispatcherTimer
      {
        Interval = TimeSpan.FromSeconds(1)
      };

      Timer.Tick += OnTimerTick;
      Timer.Start();

      UpdateClock();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);

      DragMove();
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
      UpdateClock();
    }

    private void UpdateClock()
    {
      TimerText.Text = DateTime.Now.ToString("HH:mm");
    }
  }
}
