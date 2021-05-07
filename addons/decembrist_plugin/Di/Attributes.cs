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
}