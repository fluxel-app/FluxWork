using FluxWork.Presentation.ViewModel;

namespace FluxWork.Presentation.Controller
{
  public class NavigationTargetController<TViewModel> : ControllerBase<TViewModel> where TViewModel : IViewModel
  {
    public string Title { get; set; }
  }
}
