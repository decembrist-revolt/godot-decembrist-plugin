using Decembrist.Di;
using Decembrist.Example.LanEventBusEventTest;
using Decembrist.Example.Service;

namespace Decembrist.Example
{
    public class DecembristConfiguration : IDecembristConfiguration
    {
        public ContainerBuilder ConfigDi(ContainerBuilder builder)
        {
            builder.RegisterInstance(InstanceService.Instance);
            builder.Register<LanEventBusEventServer.SerializedClassSerializer>();
            builder.Register<SingletonService3>();
            builder.Register<RootNodeService>();
            builder.Register<SingletonService2, IService>();
            builder.Register<SingletonService1>();
            builder.RegisterPrototype<PrototypeService1>();
            builder.RegisterPrototype<PrototypeService2>();
            return builder;
        }
    }
}