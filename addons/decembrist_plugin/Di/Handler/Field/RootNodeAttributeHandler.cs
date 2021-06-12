using System;
using System.Reflection;
using Godot;

namespace Decembrist.Di.Handler
{
    public class RootNodeAttributeHandler : IFieldAttributeHandler
    {
        private readonly Node _rootNode;

        public RootNodeAttributeHandler(Node rootNode)
        {
            _rootNode = rootNode;
        }

        public void Handle(object instance, FieldInfo field)
        {
            if (field.GetCustomAttribute(typeof(RootNodeAttribute)) == null) return;

            if (field.FieldType != typeof(Node))
            {
                throw new Exception($"RootNode attribute field should be Node type, for class {instance.GetType()}");
            }

            field.SetValue(instance, _rootNode);
        }
    }
}