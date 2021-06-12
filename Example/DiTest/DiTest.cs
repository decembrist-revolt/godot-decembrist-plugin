using System.Threading.Tasks;
using Decembrist.Di;
using Decembrist.Example.Service;
using Godot;
using static Decembrist.Example.Assertions;

namespace Decembrist.Example
{
    public class DiTest : Node2D, ITest
    {
        [Inject] private IService _service1;
        [Inject] private IService _service2;
        [Inject] private PrototypeService1 _prototypeService1;
        [Inject] private PrototypeService1 _prototypeService2;
        [Inject] private InstanceService _instanceService;
        [Inject] private RootNodeService _rootNodeService;
        
        [SettingsValue(DecembristSettings.EventBusEnabled)]
        private bool _eventBusEnabled;

        public override void _Ready()
        {
            this.InjectAll();
        }

        public async Task Test()
        {
            AssertNotNull(_service1, "singleton service exists");
            AssertNotNull(this.Resolve<IService>(), "singleton service exists");
            AssertTrue(_instanceService == InstanceService.Instance, "service is singleton");
            AssertTrue(this.Resolve<IService>() == _service1, "service is singleton");
            AssertTrue(_service1 == _service2, "service is singleton");
            AssertNotNull(this.Resolve<PrototypeService1>(), "prototype service exists");
            AssertTrue(this.Resolve<PrototypeService1>() != _prototypeService1, "service is prototype");
            AssertTrue(_prototypeService1 != _prototypeService2, "service is prototype");
            AssertTrue(_prototypeService1.PrototypeService != _prototypeService2.PrototypeService,
                "service.PrototypeService is prototype");
            AssertTrue(_eventBusEnabled, "settings value check");
            AssertNotNull(_rootNodeService, "root node service exists");
            AssertTrue(_rootNodeService.RootNode == GetTree().Root, "root node correct");
        }
        
    }
}