using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OverlayTimer
{
  public static class IconHelper
  {
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyIcon(IntPtr hIcon);

    public static Icon DrawingImageToIcon(
      DrawingImage drawingImage,
      int width,
      int height,
      double dpi = 96)
    {
      var drawing = drawingImage.Drawing;
      var bounds = drawing.Bounds;

      var scaleX = width / bounds.Width;
      var scaleY = height / bounds.Height;
      var scale = Math.Min(scaleX, scaleY);

      var offsetX = (width - (bounds.Width * scale)) / 2.0;
      var offsetY = (height - (bounds.Height * scale)) / 2.0;

      var rtb = new RenderTargetBitmap(
          width, height,
          dpi, dpi,
          PixelFormats.Pbgra32);

      var dv = new DrawingVisual();
      using (var ctx = dv.RenderOpen())
      {
        ctx.PushTransform(new TranslateTransform(
            offsetX - (bounds.X * scale),
            offsetY - (bounds.Y * scale)));
        ctx.PushTransform(new ScaleTransform(scale, scale));

        ctx.DrawDrawing(drawing);

        ctx.Pop();
        ctx.Pop();
      }
      rtb.Render(dv);

      var encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(rtb));
      using (var ms = new MemoryStream())
      {
        encoder.Save(ms);
        ms.Seek(0, SeekOrigin.Begin);

        using (var bmp = new Bitmap(ms))
        {
          var hIcon = bmp.GetHicon();
          try
          {
            return (Icon)Icon.FromHandle(hIcon).Clone();
          }
          finally
          {
            DestroyIcon(hIcon);
          }
        }
      }
    }

    public static void SaveIconToFile(Icon icon, string filePath)
    {
      using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
      {
        icon.Save(fs);
      }
    }
  }
}
