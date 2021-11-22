using FluxWork.Presentation.View;
using FluxWork.Presentation.ViewModel;
using System;
using System.Windows;

namespace FluxWork.Presentation.Controller
{
  public abstract class DialogController<TViewModel> : ControllerBase<TViewModel> where TViewModel : IViewModel
  {
    protected DialogController() => this.Dimensions = new DialogDimensions(this.GetType().FullName);

    public string Title { get; set; }

    public DialogDimensions Dimensions { get; }

    public void Close(MessageBoxResult result = MessageBoxResult.OK)
    {
      var closeEvent = this.CloseEvent;
      if (closeEvent == null)
        return;
      closeEvent((object) this, result);
    }

    internal event EventHandler<MessageBoxResult> CloseEvent;
  }
}
