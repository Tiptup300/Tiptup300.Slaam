using SimpleInjector;

namespace Tiptup300.Slaam.Composition.x_;


public class ServiceLocator
{
   public static ServiceLocator Instance = new ServiceLocator();

   private readonly Dictionary<Type, object> _instances;
   private readonly Container _simpleInjectorContainer;

   public ServiceLocator()
   {
      _instances = new Dictionary<Type, object>();
      _simpleInjectorContainer = new Composer().BuildContainer(this);
   }

   public T GetService<T>()
   {
      if (_instances.ContainsKey(typeof(T)) == false)
      {
         _instances.Add(typeof(T), _simpleInjectorContainer.GetInstance(typeof(T)));
      }
      return (T)_instances[typeof(T)];
   }

   public static T LocateService<T>() => Instance.GetService<T>();
}
