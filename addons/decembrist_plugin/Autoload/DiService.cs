#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using Decembrist.Di;
using Decembrist.Di.Handler;
using Decembrist.Di.Handler.Processor;
using Godot;
using DiContainer = Decembrist.Di.Container;

namespace Decembrist.Autoload
{
    public class DiService : Node
    {
        public DiContainer Container;

        private IList<IFieldAttributeHandler> _fieldAttributeHandlers;
        private IList<IDependencyPostProcessor> _postProcessors;

        public override void _Ready()
        {
            _fieldAttributeHandlers = GetFieldHandlers();
            _postProcessors = GetPostProcessors();
            var builder = new ContainerBuilder();
            RegisterPluginDependencies(builder);
            builder.SetParameterHandlers(GetParameterHandlers());
            builder = InstantiateConfig()?.ConfigDi(builder) ?? builder;
            Container = builder.Build();
        }

        public T? ResolveOrNull<T>() where T : class
        {
            return Container.ResolveOrNull(typeof(T)) as T;
        }
        
        public object? ResolveOrNull(Type type)
        {
            return Container.ResolveOrNull(type);
        }

        public void InjectAll(object instance)
        {
            HandleFieldAttributes(instance);
        }
        
        public void HandlePostProcessors()
        {
            foreach (var postProcessor in _postProcessors)
            {
                foreach (var (_, instance) in Container.InstanceMap)
                {
                    postProcessor.Process(instance);
                }
            }
        }

        private List<IFieldAttributeHandler> GetFieldHandlers() => new()
        {
            new ChildNodeAttributeHandler(),
            new RootNodeAttributeHandler(GetTree().Root)
        };
        
        private List<IParameterAttributeHandler> GetParameterHandlers() => new()
        {
            new RootNodeParameterAttributeHandler(GetTree().Root)
        };

        private List<IDependencyPostProcessor> GetPostProcessors() => new()
        {
            new PostConstructPostProcessor()
        };

        private void HandleFieldAttributes(object instance)
        {
            var type = instance.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                HandleInject(instance, field);
                HandleSettingsValue(instance, field);
                foreach (var handler in _fieldAttributeHandlers)
                {
                    handler.Handle(instance, field);
                }
            }
        }

        /// <summary>
        /// Handle [Inject] attribute <see cref="InjectAttribute"/>
        /// </summary>
        private void HandleInject(object instance, FieldInfo field)
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
                throw new UnsatisfiedDependencyException(
                    $"class {instance.GetType()}. Dependency type => {serviceType}");
            }
        }

        /// <summary>
        /// Handle [SettingsValue] attribute <see cref="SettingsValueAttribute"/>
        /// </summary>
        private void HandleSettingsValue(object instance, FieldInfo field)
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
                throw new UnsatisfiedDependencyException(
                    $"class {instance.GetType()}. Settings value => {settingsName} not found");
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
        
        private void RegisterPluginDependencies(ContainerBuilder builder)
        {
            builder.Register<EventBus>();
            if (ProjectSettings.GetSetting(DecembristSettings.LanEventsEnabled) is true)
            {
                builder.Register<LanEventBus>();
            }
        }
    }
}