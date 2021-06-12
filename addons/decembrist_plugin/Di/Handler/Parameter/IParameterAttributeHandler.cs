#nullable enable
using System.Reflection;

namespace Decembrist.Di.Handler
{
    public interface IParameterAttributeHandler
    {
        object? Handle(ParameterInfo param);
    }
}