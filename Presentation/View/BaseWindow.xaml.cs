using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;

namespace FluxWork.Presentation.View
{
  public partial class BaseWindow : Window
  {
    public BaseWindow()
    {
      this.InitializeComponent();
    }

    public void SetBackgroundImage(Image image, int m, int w, double opacity = 0.1)
    {
      foreach (UIElement element in this.ImagePanel.Children.OfType<Image>())
        this.ImagePanel.Children.Remove(element);
      image.VerticalAlignment = VerticalAlignment.Bottom;
      image.HorizontalAlignment = HorizontalAlignment.Left;
      image.Margin = new Thickness((double) m);
      this.ImageWidthDefinition.Width = new GridLength((double) w);
      image.Opacity = opacity;
      Grid.SetRow((UIElement) image, 1);
      this.ImagePanel.Children.Add((UIElement) image);
    }

    public void SetStatus(string status = null) => this.StatusLabel.Dispatcher?.Invoke((Action) (() =>
    {
      if (status == null)
      {
        this.FooterHeight.Height = new GridLength(0.0);
        this.StatusLabel.Content = (object) string.Empty;
      }
      else
      {
        this.FooterHeight.Height = new GridLength(0.0, GridUnitType.Auto);
        this.StatusLabel.Content = (object) status;
      }
    }));
  }
}
