using LightInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using FluxWork.Extensions;
using FluxWork.Presentation.Command;
using FluxWork.Presentation.Controller;
using FluxWork.Presentation.View;
using FluxWork.Presentation.ViewModel;
using FluxWork.Services;

namespace FluxWork.Controller
{
  internal class DependencyService : IDependencyService, IService
  {
    private readonly ServiceContainer _c;
    private bool _init;
    private Regex _assemblyNameFilter;
    private readonly IDictionary<string, Assembly> _knownAssemblies = (IDictionary<string, Assembly>) new Dictionary<string, Assembly>();

    public DependencyService()
    {
      this._c = new ServiceContainer();
      AnnotationExtension.EnableAnnotatedPropertyInjection(this._c);
    }

    public void SetAssemblyNameFilter(string regex) => this._assemblyNameFilter = new Regex(regex);

    private IEnumerable<Assembly> GetAssemblies()
    {
      var strings = ((IEnumerable<string>) Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "*.dll")).Where<string>((Func<string, bool>) (f => this._assemblyNameFilter == null || this._assemblyNameFilter.IsMatch(Path.GetFileNameWithoutExtension(f)) || Path.GetFileNameWithoutExtension(f) == "HitWork"));
      var assemblyList = new List<Assembly>();
      foreach (var str in strings)
      {
        if (this._knownAssemblies.ContainsKey(str))
        {
          assemblyList.Add(this._knownAssemblies[str]);
        }
        else
        {
          Assembly assembly;
          assemblyList.Add(assembly = Assembly.LoadFile(str));
          this._knownAssemblies.Add(str, assembly);
        }
      }
      if (!assemblyList.Contains(Assembly.GetEntryAssembly()))
        assemblyList.Add(Assembly.GetEntryAssembly());
      return (IEnumerable<Assembly>) assemblyList;
    }

    private IEnumerable<Type> GetTypes()
    {
      var typeList = new List<Type>();
      foreach (var assembly in this.GetAssemblies())
      {
        try
        {
          typeList.AddRange((IEnumerable<Type>) assembly.GetTypes());
        }
        catch (ReflectionTypeLoadException)
        {
        }
      }
      return (IEnumerable<Type>) typeList;
    }

    public static object PropertyChangedProxy(object o, bool strict)
    {
      var type = o.GetType();
      var str = type.FullName + "_Proxy";
      var fileName = str + ".dll";
      var typeBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(str), AssemblyBuilderAccess.RunAndSave).DefineDynamicModule(str, fileName).DefineType(type.Name + "Proxy", TypeAttributes.Public, type);
      typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
      var method = type.GetMethod("OnPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
      foreach (var propertyInfo in ((IEnumerable<PropertyInfo>) type.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p =>
      {
        if (!p.CanRead || !p.CanWrite)
          return false;
        return strict || p.GetGetMethod().IsVirtual;
      })))
      {
        var getMethod = propertyInfo.GetGetMethod();
        var methodBuilder1 = typeBuilder.DefineMethod(getMethod.Name, getMethod.Attributes, propertyInfo.PropertyType, (Type[]) null);
        var ilGenerator1 = methodBuilder1.GetILGenerator();
        ilGenerator1.Emit(OpCodes.Ldarg_0);
        ilGenerator1.EmitCall(OpCodes.Call, getMethod, (Type[]) null);
        ilGenerator1.Emit(OpCodes.Ret);
        typeBuilder.DefineMethodOverride((MethodInfo) methodBuilder1, getMethod);
        var setMethod = propertyInfo.GetSetMethod();
        var methodBuilder2 = typeBuilder.DefineMethod(setMethod.Name, setMethod.Attributes, typeof (void), new Type[1]
        {
          propertyInfo.PropertyType
        });
        var ilGenerator2 = methodBuilder2.GetILGenerator();
        ilGenerator2.Emit(OpCodes.Ldarg_0);
        ilGenerator2.Emit(OpCodes.Ldarg_1);
        ilGenerator2.Emit(OpCodes.Call, setMethod);
        ilGenerator2.Emit(OpCodes.Ldarg_0);
        ilGenerator2.Emit(OpCodes.Ldstr, propertyInfo.Name);
        var call = OpCodes.Call;
        ilGenerator2.Emit(call, method ?? throw new InvalidOperationException());
        ilGenerator2.Emit(OpCodes.Ret);
        typeBuilder.DefineMethodOverride((MethodInfo) methodBuilder2, setMethod);
      }
      return Activator.CreateInstance(typeBuilder.CreateType());
    }

    public void Init()
    {
      if (this._init)
        return;
      this.RegisterViewModels();
      this.RegisterControllers();
      this.RegisterViews();
      this.RegisterServices();
      this.RegisterCommands();
      this._init = true;
    }

    public object GetDiContainer() => (object) this._c;

    private void RegisterCommands()
    {
      foreach (var type in this.GetTypes().Where<Type>((Func<Type, bool>) (t => !t.IsAbstract && typeof (CommandBase).IsAssignableFrom(t))))
        this.Register(type);
    }

    private void RegisterServices()
    {
      var array = this.GetTypes().ToArray<Type>();
      foreach (var type1 in ((IEnumerable<Type>) array).Where<Type>((Func<Type, bool>) (t => t.IsInterface && t != typeof (IService) && typeof (IService).IsAssignableFrom(t) && t != typeof (IDependencyService))))
      {
        var serviceType = type1;
        foreach (object obj in array)
          Console.WriteLine(string.Format("{0}:{1} - ()", obj, (object) serviceType));
        var type2 = ((IEnumerable<Type>) array).Single<Type>((Func<Type, bool>) (t => !t.IsAbstract && serviceType.IsAssignableFrom(t)));
        this.RegisterSingleton(serviceType, type2);
      }
      this.RegisterSingleton<IDependencyService, DependencyService>(this);
    }

    private void RegisterViews()
    {
      foreach (var type in this.GetTypes().Where<Type>((Func<Type, bool>) (t => t.IsAssignableToGenericType(typeof (IViewFor<>)) && t.IsClass && !t.IsAbstract)))
        this.Register(type);
    }

    private void RegisterControllers()
    {
      foreach (var type in this.GetTypes().Where<Type>((Func<Type, bool>) (t => t.Assembly != this.GetType().Assembly)).Where<Type>((Func<Type, bool>) (t => t.IsClass && !t.IsAbstract && t.IsAssignableToGenericType(typeof (ControllerBase<>)))))
        this.Register(type);
    }

    private void RegisterViewModels()
    {
      foreach (var type1 in this.GetTypes().Where<Type>((Func<Type, bool>) (t => t.Assembly != this.GetType().Assembly)).Where<Type>((Func<Type, bool>) (t => t.IsClass && !t.IsAbstract && typeof (ViewModelBase).IsAssignableFrom(t))))
      {
        var type = type1;
        ServiceRegistryExtensions.Register((IServiceRegistry) this._c, type, (Func<IServiceFactory, object>) (f =>
        {
          var source = ((IEnumerable<ParameterInfo>) ((IEnumerable<ConstructorInfo>) type.GetConstructors()).Single<ConstructorInfo>().GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameterInfo => f.Create(parameterInfo.ParameterType)));
          object instance;
          if (!source.Any<object>())
            instance = Activator.CreateInstance(type);
          else
            instance = Activator.CreateInstance(type, (object) BindingFlags.CreateInstance, (object) source);
          var num = typeof (VirtualViewModelBase).IsAssignableFrom(type) ? 1 : 0;
          return DependencyService.PropertyChangedProxy(instance, num != 0);
        }));
      }
    }

    public void Register<TType>() => this.Register(typeof (TType));

    public void Register(Type type) => this._c.Register(type);

    public void Register<TService, TType>() where TType : TService => this.Register(typeof (TService), typeof (TType));

    public void Register(Type service, Type type) => this._c.Register(service, type);

    public void RegisterSingleton<TType>(TType instance) => ServiceRegistryExtensions.RegisterSingleton<TType>((IServiceRegistry) this._c, (Func<IServiceFactory, TType>) (f => instance));

    public void RegisterSingleton<TService, TType>() where TType : TService => this.RegisterSingleton(typeof (TService), typeof (TType));

    public void RegisterSingleton<TService, TType>(TType instance) where TType : TService => this.RegisterSingleton(typeof (TService), (object) instance);

    public void RegisterSingleton(Type service, Type type) => ServiceRegistryExtensions.RegisterSingleton((IServiceRegistry) this._c, service, type);

    public void RegisterSingleton(Type service, object instance) => ServiceRegistryExtensions.RegisterSingleton((IServiceRegistry) this._c, service, (Func<IServiceFactory, object>) (f => instance));

    public TType Find<TType>() => (TType) this.Find(typeof (TType));

    public object Find(Type type) => this._c.GetInstance(type);
  }
}
