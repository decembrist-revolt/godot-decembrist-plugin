using Godot;
using Godot.Collections;
using Decembrist.Autoload;
using Decembrist.Service;

namespace Decembrist.Utils
{
    public static class GlobalsUtils
    {
        public static T Resolve<T>(this Node node) where T : class
        {
            var service = node.GetNode<DiService>("/root/DI");
            return service.ResolveOrNull<T>();
        }
        
        public static void InjectAll(this Node node)
        {
            var service = node.GetNode<DiService>("/root/DI");
            service.InjectAll(node);
        }
        
        public static void UpdateConfig(this Node node, IConfig config)
        {
            var configService = node.Resolve<ConfigService>();
            configService.Update(config);
        }
        
        public static T LoadConfig<T>(this Node node) where T : class, IConfig
        {
            var configService = node.Resolve<ConfigService>();
            return (T) configService.Get<T>();
        }
    }
}