#nullable enable
using System.Reflection;
using Godot;

namespace Decembrist.Di.Handler
{
    public class RootNodeParameterAttributeHandler : IParameterAttributeHandler
    {
        private readonly Node _rootNode;

        public RootNodeParameterAttributeHandler(Node rootNode)
        {
            _rootNode = rootNode;
        }

        public object? Handle(ParameterInfo param)
        {
            var isRootNode = param.GetCustomAttribute(typeof(RootNodeAttribute)) is RootNodeAttribute;
            if (isRootNode && param.ParameterType == typeof(Viewport) || param.ParameterType == typeof(Node))
            {
                return _rootNode;
            }
            
            return null;
        }
    }
}