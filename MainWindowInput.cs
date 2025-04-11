using System.Windows;
using System.Windows.Input;

namespace OverlayTimer
{
  public partial class MainWindow : Window
  {
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);

      DragMove();
    }
  }
}
