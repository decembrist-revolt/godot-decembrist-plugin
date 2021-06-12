using System;

namespace Decembrist.Di
{
    /// <summary>
    /// Inject dependency instance for field (by field type)
    /// <para>Use <see cref="DependencyUtils.InjectAll"/> to inject</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute: Attribute
    {
    }
    
    /// <summary>
    /// Inject root node (GetNode("/root")) for field
    /// <para>Use <see cref="DependencyUtils.InjectAll"/> to inject</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RootNodeAttribute: Attribute
    {
    }
    
    /// <summary>
    /// Inject project settings for value type field
    /// <para>Use <see cref="DependencyUtils.InjectAll"/> to inject</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SettingsValueAttribute: Attribute
    {
        public readonly string Name;

        public SettingsValueAttribute(string name)
        {
            Name = name;
        }
    }
    
    /// <summary>
    /// Inject child node by child name = nodeName or child name = field name
    /// <para>Use <see cref="DependencyUtils.InjectAll"/> to inject</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ChildNodeAttribute: Attribute
    {
        public readonly string NodeName;

        public ChildNodeAttribute(string nodeName = null)
        {
            NodeName = nodeName;
        }
    }
    
    /// <summary>
    /// Invoke method after all dependencies resolved
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PostConstructAttribute: Attribute
    {
    }
}