using FluxWork.Extensions;
using FluxWork.Presentation.Controller;
using FluxWork.Presentation.ViewModel;
using FluxWork.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FluxWork.Presentation.View
{
  internal class WindowService : IWindowService, IService
  {
    private readonly IDependencyService _dependencyService;
    private int _defaultImageWidth;
    private Image _defaultImage;
    private int _defaultImageMargin;
    private double _defaultImageOpacity;

    public WindowService(IDependencyService dependencyService)
    {
      this._dependencyService = dependencyService;
      this.BaseWindow = new BaseWindow();
    }

    public string TitleSuffix { get; set; }

    public BaseWindow BaseWindow { get; }

    public void Navigate<TViewModel>(
      NavigationTargetController<TViewModel> navigationTargetController)
      where TViewModel : IViewModel
    {
      this.BaseWindow.ContentPanel.Children.Clear();
      if (this._defaultImage != null)
      {
        BaseWindow baseWindow = this.BaseWindow;
        var image = new Image();
        image.Source = this._defaultImage.Source;
        var defaultImageMargin = this._defaultImageMargin;
        var defaultImageWidth = this._defaultImageWidth;
        var defaultImageOpacity = this._defaultImageOpacity;
        baseWindow.SetBackgroundImage(image, defaultImageMargin, defaultImageWidth, defaultImageOpacity);
      }
      if (!((IViewFor<TViewModel>) this._dependencyService.Find(TypeExtensions.FindTypeForView<TViewModel>()) is UserControl userControl))
        throw new Exception("View isn't User Control");
      userControl.DataContext = (object) navigationTargetController.ViewModel;
      userControl.Loaded += (RoutedEventHandler) ((sender, args) => ((ControllerBase<TViewModel>) navigationTargetController).Loaded());
      navigationTargetController.Initialized();
      this.BaseWindow.Title = navigationTargetController.Title + " | " + this.TitleSuffix;
      this.BaseWindow.ContentPanel.Children.Add((UIElement) userControl);
    }

    public MessageBoxResult Open<TViewModel>(
      DialogController<TViewModel> dialogController)
      where TViewModel : IViewModel
    {
      BaseWindow baseWindow = new BaseWindow();
      if (this._defaultImage != null)
      {
        BaseWindow baseWindow1 = baseWindow;
        var image = new Image();
        image.Source = this._defaultImage.Source;
        var defaultImageMargin = this._defaultImageMargin;
        var defaultImageWidth = this._defaultImageWidth;
        var defaultImageOpacity = this._defaultImageOpacity;
        baseWindow1.SetBackgroundImage(image, defaultImageMargin, defaultImageWidth, defaultImageOpacity);
      }
      if (!((IViewFor<TViewModel>) this._dependencyService.Find(TypeExtensions.FindTypeForView<TViewModel>()) is UserControl userControl))
        throw new Exception("View isn't User Control");
      userControl.DataContext = (object) dialogController.ViewModel;
      userControl.Loaded += (RoutedEventHandler) ((sender, args) => ((ControllerBase<TViewModel>) dialogController).Loaded());
      dialogController.Initialized();
      baseWindow.Title = dialogController.Title + " | " + this.TitleSuffix;
      baseWindow.ContentPanel.Children.Add((UIElement) userControl);
      DialogDimensions dimensions = dialogController.Dimensions;
      if (dimensions.X == 0 && dimensions.Y == 0)
      {
        var primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
        var primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
        double width = baseWindow.Width;
        double height = baseWindow.Height;
        baseWindow.Left = primaryScreenWidth / 2.0 - width / 2.0;
        baseWindow.Top = primaryScreenHeight / 2.0 - height / 2.0;
      }
      else
      {
        baseWindow.Left = (double) dimensions.X;
        baseWindow.Top = (double) dimensions.Y;
      }
      baseWindow.Width = (double) dimensions.Width;
      baseWindow.Height = (double) dimensions.Height;
      baseWindow.MinWidth = (double) dimensions.MinWidth;
      baseWindow.MinHeight = (double) dimensions.MinHeight;
      baseWindow.MaxWidth = (double) dimensions.MaxWidth;
      baseWindow.MaxHeight = (double) dimensions.MaxHeight;
      baseWindow.ResizeMode = dimensions.Width != dimensions.MinWidth || dimensions.Width != dimensions.MaxWidth || dimensions.Height != dimensions.MinHeight || dimensions.Height != dimensions.MaxHeight ? ResizeMode.CanResize : ResizeMode.NoResize;
      var result = MessageBoxResult.OK;
      dialogController.CloseEvent += (EventHandler<MessageBoxResult>) ((s, r) =>
      {
        result = r;
        baseWindow.Close();
      });
      baseWindow.ShowDialog();
      return result;
    }

    public MessageBoxResult Open(IDialogController dialogController)
    {
      var viewModelType = dialogController.GetType().BaseType.GenericTypeArguments[0];
      var method = this.GetType().GetMethods().Single(m => m.Name == "Open" && m.IsGenericMethod);
      return (MessageBoxResult) method.MakeGenericMethod(viewModelType).Invoke(this, new [] {dialogController});
    }

    public void SetDefaultBackgroundImage(Image image, int m, int w, double opacity = 0.1)
    {
      this._defaultImage = image;
      this._defaultImageMargin = m;
      this._defaultImageWidth = w;
      this._defaultImageOpacity = opacity;
    }
  }
}
