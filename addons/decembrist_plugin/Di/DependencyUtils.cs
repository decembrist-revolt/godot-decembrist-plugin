using Decembrist.Autoload;
using Godot;

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
        public static T Resolve<T>(this Node node) where T : class
        {
            var service = node.GetNode<DiService>("/root/DI");
            return service.ResolveOrNull<T>();
        }
        
        /// <summary>
        /// Inject all fields of node
        /// </summary>
        /// <param name="node">some node</param>
        public static void InjectAll(this Node node)
        {
            var service = node.GetNode<DiService>("/root/DI");
            service.InjectAll(node);
        }
        
        // public static void UpdateConfig(this Node node, IConfig config)
        // {
        //     var configService = node.Resolve<ConfigService>();
        //     configService.Update(config);
        // }
        //
        // public static T LoadConfig<T>(this Node node) where T : class, IConfig
        // {
        //     var configService = node.Resolve<ConfigService>();
        //     return (T) configService.Get<T>();
        // }
    }
}