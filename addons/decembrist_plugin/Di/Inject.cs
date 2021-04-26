using System;

namespace Decembrist.Di
{
    /// <summary>
    /// Inject dependency instance for field (by field type)
    /// Use <see cref="DependencyUtils.InjectAll"/> to inject
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class Inject: Attribute
    {
        
    }
}