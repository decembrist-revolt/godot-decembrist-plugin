#nullable enable
using System;
using System.Reflection;
using Godot;
using Decembrist.Di;
using Decembrist.Service;
using DiContainer = Decembrist.Di.Container;

namespace Decembrist.Autoload
{
    public class DiService : Node
    {
        public readonly DiContainer Container;

        public DiService()
        {
            var builder = new ContainerBuilder();
            builder.Register<EventBus>();
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
                HandleInject(instance, field, type);
                HandleSettingsValue(instance, field, type);
            }
        }

        /// <summary>
        /// Handle [Inject] attribute <see cref="InjectAttribute"/>
        /// </summary>
        private void HandleInject(object instance, FieldInfo field, Type? type)
        {
            if (field.GetCustomAttribute(typeof(InjectAttribute)) == null) return;

            var serviceType = field.FieldType;
            var service = Container.ResolveOrNull(serviceType);
            if (service != null)
            {
                field.SetValue(instance, service);
            }
            else
            {
                throw new UnsatisfiedDependencyException($"class {type}. Dependency type => {serviceType}");
            }
        }

        /// <summary>
        /// Handle [SettingsValue] attribute <see cref="SettingsValueAttribute"/>
        /// </summary>
        private void HandleSettingsValue(object instance, FieldInfo field, Type type)
        {
            var attribute = field.GetCustomAttribute(typeof(SettingsValueAttribute));
            if (attribute is not SettingsValueAttribute settingsAttribute) return;

            var settingsName = settingsAttribute.Name;
            var settingsValue = ProjectSettings.GetSetting(settingsName);
            var fieldType = field.FieldType;
            if (settingsValue != null && fieldType.IsInstanceOfType(settingsValue))
            {
                field.SetValue(instance, settingsValue);
            }
            else
            {
                throw new UnsatisfiedDependencyException($"class {type}. Settings value => {settingsName} not found");
            }
        }

        private IDecembristConfiguration? InstantiateConfig()
        {
            if (ProjectSettings.GetSetting(DecembristSettings.ConfigClass) is not string configClass)
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