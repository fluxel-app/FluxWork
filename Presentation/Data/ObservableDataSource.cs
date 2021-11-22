using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FluxWork.Presentation.Data
{
  public class ObservableDataSource<T> : ObservableCollection<T>
  {
    private readonly Func<IEnumerable<T>> _data;

    public ObservableDataSource(Func<IEnumerable<T>> data) => this._data = data;

    public void Reload()
    {
      var list = this._data().ToList<T>();
      foreach (var obj in this.Items.ToList<T>())
      {
        if (list.Contains(obj))
          list.Remove(obj);
        else
          this.Remove(obj);
      }
      foreach (var obj in list)
        this.Add(obj);
    }
  }
}
