using Decembrist.Di;
using Decembrist.Example.Service;

namespace Decembrist.Example
{
    public class DecembristConfiguration: IDecembristConfiguration
    {
        public ContainerBuilder ConfigDi(ContainerBuilder builder)
        {
            builder.Register<SomeService2, IService>();
            builder.Register<SomeService1>();
            return builder;
        }
    }
}