#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Decembrist.Service;

namespace Decembrist.Di
{
    public class Container
    {
        private readonly Dictionary<Type, object> _instanceMap;

        public Container(Dictionary<Type, object> instanceMap, Dictionary<Type, Type> typeMap)
        {
            _instanceMap = instanceMap;
            InstantiateTypes(typeMap);
        }

        public object? ResolveOrNull(Type type)
        {
            return _instanceMap.ContainsKey(type) ? _instanceMap[type] : null;
        }

        private void InstantiateTypes(Dictionary<Type, Type> typeMap)
        {
            var iteration = typeMap.Count;
            while (typeMap.Count > 0 && iteration == typeMap.Count)
            {
                var toRemove = new List<Type>();
                foreach (var (type, asType) in typeMap)
                {
                    // Instantiate through no arg constructor  
                    var instance = type.GetConstructor(new Type[0])?.Invoke(null);
                    if (instance == null)
                    {
                        var ctr = type.GetConstructors().First();
                        instance = InstantiateTypes(ctr);
                    }

                    if (instance != null)
                    {
                        _instanceMap[asType] = instance;
                        toRemove.Add(type);
                    }
                }
                foreach (var type in toRemove)
                {
                    typeMap.Remove(type);
                }

                iteration--;
            }

            if (typeMap.Count != 0)
            {
                var typesArr = typeMap.Values.Select(type => type.ToString()).ToArray();
                throw new Exception($"Unsatisfied dependencies {string.Join("", typesArr)}");
            }
        }

        private object? InstantiateTypes(ConstructorInfo ctr)
        {
            var parameters = ctr.GetParameters();
            var paramArgs = new List<object>(parameters.Length);
            foreach (var parameter in parameters)
            {
                var paramType = parameter.ParameterType;
                if (_instanceMap.ContainsKey(paramType))
                {
                    paramArgs.Add(_instanceMap[paramType]);
                }
                else
                {
                    break;
                }

            }
            
            object? result = null;
            if (parameters.Length == paramArgs.Count)
            {
                result = ctr.Invoke(paramArgs.ToArray());
            }

            return result;
        }
    }

    public class ContainerBuilder
    {
        private readonly Dictionary<Type, object> _instanceMap = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

        public ContainerBuilder RegisterInstance<T>(T instance)
        {
            if (instance == null) throw new Exception("Null instance registration");

            _instanceMap[typeof(T)] = instance;
            return this;
        }

        public void Register<T>()
        {
            _typeMap[typeof(T)] = typeof(T);
        }

        public void Register<TYpe, TYpeAs>()
        {
            _typeMap[typeof(TYpe)] = typeof(TYpeAs);
        }

        public Container Build() => new Container(_instanceMap, _typeMap);
    }
}