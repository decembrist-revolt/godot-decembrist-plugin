#nullable enable
using System;
using System.Reflection;
using Godot;
using Decembrist.Di;
using Decembrist.Service;
using DiContainer = Decembrist.Di.Container;

namespace Decembrist.Autoload
{
    public class DiService : Node2D
    {
        public DiContainer Container;

        public override void _Ready()
        {
        }

        public DiService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConfigService());
            builder = InstantiateConfig()?.ConfigDi(builder) ?? builder;
            Container = builder.Build();
        }

        public T? ResolveOrNull<T>() where T : class
        {
            return Container.ResolveOrNull(typeof(T)) as T;
        }

        public void InjectAll(object instance)
        {
            var type = instance.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute(typeof(Inject)) != null)
                {
                    var serviceType = field.FieldType;
                    var service = Container.ResolveOrNull(serviceType);
                    if (service != null)
                    {
                        field.SetValue(instance, service);
                    }
                    else
                    {
                        GD.PrintErr($"No such type registered in container: {serviceType} for class {type}");
                    }
                }
            }
        }

        private IDecembristConfiguration? InstantiateConfig()
        {
            if (!(ProjectSettings.GetSetting("decembrist/config_class") is string configClass))
            {
                return null;
            }
            var configType = Type.GetType(configClass);
            if (configType == null)
            {
                return null;
            }

            var noArgConstructor = configType?.GetConstructor(new Type[0]);
            var config = noArgConstructor?.Invoke(null) ??
                         throw new Exception($"Decembrist config {configType} has no default constructor");
            if (config is IDecembristConfiguration decembristConfiguration)
            {
                return decembristConfiguration;
            }

            return null;
        }
    }
}