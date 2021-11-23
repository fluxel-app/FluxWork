using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FluxWork.Presentation.Command;
using FluxWork.Presentation.ViewModel;
using LightInject;

namespace FluxWork.Presentation.Controller
{
  public abstract class ControllerBase<TViewModel> where TViewModel : IViewModel
  {
    private TViewModel _viewModel;
    private event PropertyChangedEventHandler ViewModelPropertyChanged;

    protected ControllerBase()
    {
      this.VirtualOnListeners = this.BuildOnListeners();
      this.ViewModelPropertyChanged += OnViewModelPropertyChanged;
    }

    private void BuildCommandListeners()
    {
      var type = this.GetType();
      foreach (var propertyInfo in typeof(TViewModel).GetProperties())
      {
        if(!typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType))
          continue;
        var method = type.GetMethod($"On{propertyInfo.Name}");
        if (method == null)
          continue;
        var parameters = method.GetParameters();
        if (parameters.Length == 1)
        {
          propertyInfo.SetValue(this.ViewModel, new DelegateCommand((o) => method.Invoke(this, new[] {o})));
        } 
        else if (parameters.Length == 0)
        {
          propertyInfo.SetValue(this.ViewModel, new DelegateCommand(() => method.Invoke(this, Array.Empty<object>())));
        }
      }
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.VirtualOnListeners.ContainsKey(e.PropertyName))
      {
        this.VirtualOnListeners[e.PropertyName].Invoke();
      }
    }

    private IReadOnlyDictionary<string, Action> VirtualOnListeners { get; }
    
    private IReadOnlyDictionary<string, Action> BuildOnListeners()
    {
      var dict = new Dictionary<string, Action>();
      var type = this.GetType();
      foreach (var propertyInfo in typeof(TViewModel).GetProperties())
      {
        var methods = new[]
        {
          type.GetMethod($"On{propertyInfo.Name}Changed", Type.EmptyTypes),
          type.GetMethod($"On{propertyInfo.Name}Changed", new[] {propertyInfo.PropertyType})
        }.Where(m => m != null).ToArray();
        if (!methods.Any())
        {
          continue;
        }
        
        dict.Add(propertyInfo.Name, () =>
        {
          foreach (var methodInfo in methods)
          {
            methodInfo.Invoke(this,
              methodInfo.GetParameters().Any()
                ? new[] {propertyInfo.GetValue(this._viewModel)}
                : Array.Empty<object>());
          }
        });
      }

      return dict;
    }

    [Inject]
    public TViewModel ViewModel
    {
      get => _viewModel;
      set
      {
        if (_viewModel != null)
        {
          _viewModel.PropertyChanged -= this.ViewModelPropertyChanged;
        }
        _viewModel = value;
        _viewModel.PropertyChanged += this.ViewModelPropertyChanged;
        this.BuildCommandListeners();
      }
    }

    public virtual void Initialized()
    {
    }

    public virtual void Loaded()
    {
    }
  }
}
