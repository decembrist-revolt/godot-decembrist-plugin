using System;
using Decembrist.Autoload;
using Godot;

#nullable enable
namespace Decembrist.Di
{
    public static class DependencyUtils
    {
        /// <summary>
        /// Resolve dependency of type {T}
        /// </summary>
        /// <param name="node">some node</param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <returns>dependency instance</returns>
        public static T? Resolve<T>(this Node node) where T : class => node.GetDiService().ResolveOrNull<T>();

        /// <summary>
        /// Resolve dependency of type <paramref name="type"/>
        /// </summary>
        /// <param name="node">Some node</param>
        /// <param name="type">Dependency type</param>
        /// <returns>Dependency instance</returns>
        public static object? Resolve(this Node node, Type type) => node.GetDiService().ResolveOrNull(type);

        /// <summary>
        /// Inject all fields of node
        /// </summary>
        /// <param name="node">Some node</param>
        public static void InjectAll(this Node node) => node.GetDiService().InjectAll(node);
    }
}