#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Decembrist.Service;

namespace Decembrist.Di
{
    /// <summary>
    /// Dependency resolver class
    /// </summary>
    public class Container
    {
        public delegate object InstanceProducer();

        private readonly Dictionary<Type, object> _instanceMap = new();
        
        /// <param name="instanceMap">Resolved instances</param>
        /// <param name="typeMap">Unresolved dependencies</param>
        internal Container(Dictionary<Type, Dependency> instanceMap, Dictionary<Type, Dependency> typeMap)
        {
            foreach (var (type, dependency) in instanceMap)
            {
                _instanceMap[type] = dependency.Instance!;
            }

            InstantiateTypes(typeMap);
        }

        /// <summary>
        /// Resolves dependency instance by type
        /// </summary>
        /// <param name="type">dependency type</param>
        /// <returns>instance or null</returns>
        public object? ResolveOrNull(Type type)
        {
            var instance = _instanceMap.ContainsKey(type) ? _instanceMap[type] : null;
            object? result;
            if (instance is InstanceProducer instanceProducer)
            {
                result = instanceProducer();
            }
            else
            {
                result = instance;
            }

            return result;
        }

        /// <summary>
        /// Instantiate unresolved dependencies
        /// </summary>
        /// <param name="typeMap">unresolved dependencies</param>
        /// <exception cref="Exception">if scope unsupported</exception>
        private void InstantiateTypes(Dictionary<Type, Dependency> typeMap)
        {
            var iteration = typeMap.Count;
            while (typeMap.Count > 0 && iteration >= 0)
            {
                var toRemove = new List<Type>();
                foreach (var (type, dependency) in typeMap)
                {
                    var instance = InstantiateType(type);

                    if (instance != null)
                    {
                        var resolvedDependency = dependency.Scope switch
                        {
                            DependencyScope.Singleton => instance,
                            DependencyScope.Prototype => new InstanceProducer(() => InstantiateType(type)),
                            _ => throw new Exception("Unsupported scope exception")
                        };
                        _instanceMap[dependency.AsType] = resolvedDependency;
                        _instanceMap[dependency.Type] = resolvedDependency;

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
                ThrowUnsatisfiedDependenciesException(typeMap);
            }
        }

        private object? InstantiateType(Type type)
        {
            // Instantiate through no arg constructor  
            var instance = type.GetConstructor(new Type[0])?.Invoke(null);
            if (instance == null)
            {
                var ctr = type.GetConstructors().First();
                instance = InstantiateConstructor(ctr);
            }

            return instance;
        }

        private void ThrowUnsatisfiedDependenciesException(Dictionary<Type, Dependency> typeMap)
        {
            var typesArr = typeMap.Values
                .Select(type => type.ToString())
                .ToArray();
            throw new Exception($"Unsatisfied dependencies for [{string.Join(", ", typesArr)}]");
        }

        /// <summary>
        /// Instantiate from constructor by params
        /// </summary>
        /// <param name="ctr">constructor</param>
        /// <returns>instantiated object or null</returns>
        private object? InstantiateConstructor(ConstructorInfo ctr)
        {
            var parameters = ctr.GetParameters();
            var paramArgs = new List<object>(parameters.Length);
            foreach (var parameter in parameters)
            {
                var paramType = parameter.ParameterType;
                if (_instanceMap.ContainsKey(paramType))
                {
                    var instance = ResolveOrNull(paramType)!;
                    paramArgs.Add(instance);
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

        /// <summary>
        /// Dependency storage
        /// </summary>
        internal class Dependency
        {
            public Type Type { get; }
            /// <summary>
            /// Dependency type registered for
            /// </summary>
            public Type AsType { get; }
            /// <summary>
            /// Instance != null if dependency resolved
            /// </summary>
            public object? Instance { get; }
            public DependencyScope Scope { get; }

            private Dependency(object? instance, Type type, DependencyScope scope, Type? asType = null)
            {
                if (!ValidAsType(type, asType))
                {
                    throw new Exception($"{type} should be subtype of {asType}");
                }

                Type = type;
                Instance = instance;
                Scope = scope;
                AsType = asType ?? type;
            }

            private static bool ValidAsType(Type type, Type? asType)
            {
                return asType == null 
                       || asType == type 
                       || type.IsSubclassOf(asType) 
                       || type.IsInstanceOfType(asType)
                       || type.GetInterface(asType.Name) != null;
            }

            public Dependency(object instance, DependencyScope scope, Type? asType = null) : this(
                instance ?? throw new Exception("Unknown dependency type"),
                instance.GetType(),
                scope,
                asType
            )
            {
            }

            public Dependency(Type type, DependencyScope scope, Type? asType = null) : this(
                null,
                type ?? throw new Exception("Unknown dependency type"),
                scope,
                asType
            )
            {
            }

            public override string ToString()
            {
                return $"{{\"type\": \"{Type}\", \"asType\": \"{AsType}\"}}";
            }
        }
    }
}