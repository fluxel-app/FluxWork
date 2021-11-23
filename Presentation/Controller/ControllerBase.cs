using System;
using System.ComponentModel;
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
      this.ViewModelPropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      var type = this.GetType();
      var viewModelType = typeof(TViewModel);
      
      var propertyInfo = viewModelType.GetProperty(e.PropertyName);
      if (propertyInfo == null)
      {
        return;
      }

      var methodInfo = type.GetMethod($"On{e.PropertyName}Changed", new[] {propertyInfo.PropertyType});
      if (methodInfo == null)
      {
        return;
      }

      methodInfo.Invoke(this, new[] {propertyInfo.GetValue(this._viewModel)});
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
