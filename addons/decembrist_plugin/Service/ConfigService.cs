using Godot;

namespace Decembrist.Service
{
    public class ConfigService : Node
    {
        private IConfig _config;

        public void Update(IConfig config) => _config = config;

        public object Get<T>() where T : class, IConfig => _config as T;
    }

    public interface IConfig
    {
    }
}