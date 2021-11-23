using FluxWork.Presentation.Controller;
using FluxWork.Presentation.View;
using FluxWork.Presentation.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FluxWork.Services
{
  public interface IWindowService : IService
  {
    void Navigate<TViewModel>(
      NavigationTargetController<TViewModel> navigationTargetController)
      where TViewModel : IViewModel;

    MessageBoxResult Open<TViewModel>(DialogController<TViewModel> dialogController) where TViewModel : IViewModel;
    
    MessageBoxResult Open(IDialogController dialogController);

    void SetDefaultBackgroundImage(Image image, int m, int w, double opacity = 0.1);

    string TitleSuffix { get; set; }

    BaseWindow BaseWindow { get; }
  }
}
