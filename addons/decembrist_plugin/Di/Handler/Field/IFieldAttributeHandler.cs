#nullable enable
using System.Reflection;

namespace Decembrist.Di.Handler
{
    public interface IFieldAttributeHandler
    {
        void Handle(object instance, FieldInfo field);
    }
}