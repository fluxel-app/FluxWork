using FluxWork.Presentation.ViewModel;

namespace FluxWork.Presentation.Controller
{
  public abstract class ContentController<TViewModel> : ControllerBase<TViewModel> where TViewModel : IViewModel
  {
  }
}
