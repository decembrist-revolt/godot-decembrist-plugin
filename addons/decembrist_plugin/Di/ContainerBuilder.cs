#nullable enable
using System;
using System.Collections.Generic;
using Dependency = Decembrist.Di.Container.Dependency;

namespace Decembrist.Di
{
    /// <summary>
    /// Builder for <see cref="Container"/>
    /// </summary>
    public class ContainerBuilder
    {
        private readonly Dictionary<Type, Dependency> _instanceMap = new();
        private readonly Dictionary<Type, Dependency> _typeMap = new();

        /// <summary>
        /// Register singleton instance
        /// </summary>
        /// <param name="instance">instance to register</param>
        /// <typeparam name="T">instance Type</typeparam>
        /// <returns>this ContainerBuilder</returns>
        /// <exception cref="Exception">If instance is null</exception>
        public ContainerBuilder RegisterInstance<T>(T instance)
        {
            if (instance == null) throw new Exception("Null instance registration");

            _instanceMap[typeof(T)] = new Dependency(instance, DependencyScope.Singleton);
            return this;
        }

        /// <summary>
        /// Register new dependency of T type
        /// </summary>
        /// <param name="scope">dependency scope <see cref="DependencyScope"/></param>
        /// <typeparam name="T">dependency class type</typeparam>
        /// <returns>this ContainerBuilder</returns>
        public ContainerBuilder Register<T>(DependencyScope scope = DependencyScope.Singleton) => Register<T, T>(scope);

        /// <summary>
        /// Register new dependency T for TA type
        /// </summary>
        /// <param name="scope">dependency scope <see cref="DependencyScope"/></param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <typeparam name="TA">Type to inject to</typeparam>
        /// <returns>this ContainerBuilder</returns>
        public ContainerBuilder Register<T, TA>(DependencyScope scope = DependencyScope.Singleton)
        {
            _typeMap[typeof(T)] = new Dependency(typeof(T), scope, typeof(TA));
            return this;
        }

        /// <summary>
        /// <see cref="Register{T}"/> with prototype dependency scope
        /// 
        /// see <see cref="DependencyScope.Prototype"/>
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <returns>this ContainerBuilder</returns>
        public ContainerBuilder RegisterPrototype<T>() => Register<T>(DependencyScope.Prototype);
        
        /// <summary>
        /// <see cref="Register{T, TA}"/> with prototype dependency scope <see cref="DependencyScope.Prototype"/>
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <typeparam name="TA">Type to inject to</typeparam>
        /// <returns>this ContainerBuilder</returns>
        public ContainerBuilder RegisterPrototype<T, TA>() => Register<T, TA>(DependencyScope.Prototype);

        internal Container Build() => new(_instanceMap, _typeMap);
        
    }

    
}