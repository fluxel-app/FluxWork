using System;

namespace FluxWork.Services
{
  public interface IDependencyService : IService
  {
    void SetAssemblyNameFilter(string regex);

    void Register<TType>();

    void Register(Type type);

    void Register<TService, TType>() where TType : TService;

    void Register(Type service, Type type);

    void RegisterSingleton<TType>(TType instance);

    void RegisterSingleton<TService, TType>() where TType : TService;

    void RegisterSingleton<TService, TType>(TType instance) where TType : TService;

    void RegisterSingleton(Type service, Type type);

    void RegisterSingleton(Type service, object instance);

    object Find(Type type);

    TType Find<TType>();

    void Init();

    object GetDiContainer();
  }
}
