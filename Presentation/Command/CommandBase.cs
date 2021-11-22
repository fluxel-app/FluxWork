using System;
using System.Windows.Input;

namespace FluxWork.Presentation.Command
{
  public abstract class CommandBase : ICommand
  {
    public virtual bool CanExecute(object parameter) => true;

    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged;

    public void OnCanExecuteChanged()
    {
      var canExecuteChanged = this.CanExecuteChanged;
      if (canExecuteChanged == null)
        return;
      canExecuteChanged((object) this, EventArgs.Empty);
    }
  }

  public abstract class CommandBase<T> : CommandBase
  {
    public override void Execute(object parameter) => this.Execute((T) parameter);

    public abstract void Execute(T parameter);
  }
}