using FluxWork.Presentation.ViewModel;
using LightInject;

namespace FluxWork.Presentation.Controller
{
  public abstract class ControllerBase<TViewModel> where TViewModel : IViewModel
  {
    [Inject]
    public TViewModel ViewModel { get; set; }

    public virtual void Initialized()
    {
    }

    public virtual void Loaded()
    {
    }
  }
}
