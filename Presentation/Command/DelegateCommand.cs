using System;
using System.Windows.Input;

namespace FluxWork.Presentation.Command
{
  public class DelegateCommand<T> : ICommand where T : class
  {
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public event EventHandler CanExecuteChanged;

    public DelegateCommand(Action<T> execute)
      : this(execute, (Predicate<T>) null)
    {
    }

    public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
    {
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => this._canExecute == null || this._canExecute(parameter as T);

    public void Execute(object parameter) => this._execute(parameter as T);

    public void RaiseCanExecuteChanged()
    {
      if (this.CanExecuteChanged == null)
        return;
      this.CanExecuteChanged((object) this, EventArgs.Empty);
    }
  }
  
  public class DelegateCommand : DelegateCommand<object>
  {
    public DelegateCommand(Action<object> execute)
      : base(execute)
    {
    }

    public DelegateCommand(Action execute)
      : base((Action<object>) (e => execute()))
    {
    }

    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
      : base(execute, canExecute)
    {
    }
  }
}
