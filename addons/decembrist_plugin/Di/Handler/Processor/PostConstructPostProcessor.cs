using System;
using System.Reflection;

namespace Decembrist.Di.Handler.Processor
{
    public class PostConstructPostProcessor : IDependencyPostProcessor
    {
        public void Process(object instance)
        {
            var type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute(typeof(PostConstructAttribute)) == null) continue;

                if (method.GetParameters().Length > 0)
                {
                    throw new Exception("[PostConstruct] method must have no parameters");
                }
                method.Invoke(instance, Array.Empty<object>());
            }
        }
    }
}