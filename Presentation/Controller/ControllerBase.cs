// Decompiled with JetBrains decompiler
// Type: HitWork.Presentation.Controller.ControllerBase`1
// Assembly: HitWork, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 144200C0-2ED8-4E04-9EBD-E00A6DB6EB6D
// Assembly location: C:\Users\Hendrik Heinle\RiderProjects\Edently.ZaWin\lib\HitWork.dll

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
