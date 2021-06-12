using System;
using System.Reflection;
using Godot;

namespace Decembrist.Di.Handler
{
    public class ChildNodeAttributeHandler : IFieldAttributeHandler
    {
        public void Handle(object instance, FieldInfo field)
        {
            if (field.GetCustomAttribute(typeof(ChildNodeAttribute)) is not ChildNodeAttribute attribute) return;

            if (instance is not Node node)
            {
                throw new Exception($"ChildNode attribute used on not node class: {instance}");
            }

            var nodeName = GetNodeName(attribute, field.Name);
            var childNode = node.GetNode(nodeName);
            if (childNode == null)
            {
                throw new Exception($"ChildNode {instance.GetType()}.{nodeName} is null");
            }

            var fieldType = field.FieldType;
            if (!fieldType.IsInstanceOfType(childNode))
            {
                throw new Exception($"ChildNode {instance.GetType()}.{nodeName} type {fieldType} expected");
            }

            field.SetValue(node, childNode);
        }

        private string GetNodeName(ChildNodeAttribute attribute, string fieldName)
        {
            var nodeName = attribute.NodeName;
            if (!string.IsNullOrEmpty(nodeName)) return nodeName;

            nodeName = HandlePrivateFieldNameConvention(fieldName, nodeName);
            return nodeName ?? fieldName;
        }

        private string HandlePrivateFieldNameConvention(string fieldName, string nodeName)
        {
            if (fieldName.StartsWith("_") && fieldName.Length >= 2)
            {
                nodeName = char.ToUpper(fieldName[1]) + fieldName[2..];
            }

            return nodeName;
        }
    }
}