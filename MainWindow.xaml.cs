﻿using System;
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
    private readonly DispatcherTimer Timer;

    private NotifyIcon TrayIcon;

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
      CreateTrayIcon();
    }

    private void CreateTrayIcon()
    {
      var menu = new ContextMenuStrip();
      menu.Items.Add("Open", null, (s, e) => ShowMainWindow());
      menu.Items.Add("Hide", null, (s, e) => HideMainWindow());
      menu.Items.Add("Exit", null, (s, e) => Close());

      TrayIcon = new NotifyIcon();
      TrayIcon.Text = Title;
      TrayIcon.Visible = true;
      TrayIcon.MouseClick += (s, e) => OnMouseClick(e);
      TrayIcon.ContextMenuStrip = menu;
      TrayIcon.Icon = IconHelper.DrawingImageToIcon((DrawingImage)Icon, 256, 256);
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
