using FluxWork.Presentation.View;
using FluxWork.Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluxWork.Extensions
{
  public static class TypeExtensions
  {
    public static Type FindTypeForView<TViewModel>() where TViewModel : IViewModel => ((IEnumerable<Type>) Assembly.GetAssembly(typeof (TViewModel)).GetTypes()).Single<Type>((Func<Type, bool>) (myType => myType.IsClass && !myType.IsAbstract && typeof (IViewFor<TViewModel>).IsAssignableFrom(myType)));

    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
      if (givenType.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == genericType))
      {
        return true;
      }
      if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        return true;
      var baseType = givenType.BaseType;
      return !(baseType == (Type) null) && baseType.IsAssignableToGenericType(genericType);
    }
  }
}
