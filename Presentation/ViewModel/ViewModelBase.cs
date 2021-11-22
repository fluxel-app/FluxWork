using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluxWork.Presentation.ViewModel
{
  public class ViewModelBase : IViewModel, INotifyPropertyChanged
  {
    private readonly IDictionary<string, object> _propertyValues = (IDictionary<string, object>) new Dictionary<string, object>();

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var propertyChanged = this.PropertyChanged;
      propertyChanged?.Invoke((object) this, new PropertyChangedEventArgs(propertyName));
    }

    protected void Set<T>(ref T f, T v, [CallerMemberName] string propertyName = null)
    {
      if (propertyName == null)
        throw new ArgumentException();
      f = v;
      this.OnPropertyChanged(propertyName);
    }

    protected void Set<T>(T v, [CallerMemberName] string propertyName = null)
    {
      if (propertyName == null)
        throw new ArgumentException();
      this._propertyValues[propertyName] = (object) v;
      this.OnPropertyChanged(propertyName);
    }

    protected T Get<T>([CallerMemberName] string propertyName = null) => !this._propertyValues.ContainsKey(propertyName) ? default (T) : (T) this._propertyValues[propertyName];

    protected void Set(object v, [CallerMemberName] string propertyName = null)
    {
      if (propertyName == null)
        throw new ArgumentException();
      this._propertyValues[propertyName] = v;
      this.OnPropertyChanged(propertyName);
    }

    protected object Get([CallerMemberName] string propertyName = null) => !this._propertyValues.ContainsKey(propertyName) ? (object) null : this._propertyValues[propertyName];
  }
}
