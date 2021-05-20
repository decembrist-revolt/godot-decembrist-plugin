using Decembrist.Di;
using Decembrist.Example.Service;
using Godot;
using static Decembrist.Example.Assertions;

namespace Decembrist.Example
{
    public class DiTestNode : Node2D
    {
        [Inject] private IService _service1;

        [Inject] private IService _service2;

        [Inject] private PrototypeService1 _prototypeService1;

        [Inject] private PrototypeService1 _prototypeService2;

        [Inject] private InstanceService _instanceService;
        
        [SettingsValue(DecembristSettings.EventBusEnabled)]
        private bool _eventBusEnabled;

        public override void _Ready()
        {
            // init dependencies
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            this.InjectAll();
            watch.Stop();
            GD.Print($"Injection time Time: {watch.ElapsedMilliseconds} ms");
            GD.Print("Service assertions................................................");
            AssertTrue(_service1 != null, "singleton service exists");
            AssertTrue(this.Resolve<IService>() != null, "singleton service exists");
            AssertTrue(_instanceService == InstanceService.Instance, "service is singleton");
            AssertTrue(this.Resolve<IService>() == _service1, "service is singleton");
            AssertTrue(_service1 == _service2, "service is singleton");
            AssertTrue(this.Resolve<PrototypeService1>() != null, "prototype service exists");
            AssertTrue(this.Resolve<PrototypeService1>() != _prototypeService1, "service is prototype");
            AssertTrue(_prototypeService1 != _prototypeService2, "service is prototype");
            AssertTrue(_prototypeService1.PrototypeService != _prototypeService2.PrototypeService,
                "service.PrototypeService is prototype");
            AssertTrue(_eventBusEnabled, "settings value check");
            GD.Print("Service test......................................................");
            ServiceEcho();
        }

        private void ServiceEcho()
        {
            GD.Print($"singleton1 says              {_service1.GetString()}");
            GD.Print($"singleton2 says              {_service2.GetString()}");
            GD.Print($"resolved singleton says      {this.Resolve<IService>().GetString()}");
            GD.Print($"prototype1 says              {_prototypeService1.GetString()}");
            GD.Print($"prototype2 says              {_prototypeService2.GetString()}");
            GD.Print($"resolved prototype says      {this.Resolve<PrototypeService1>().GetString()}");
        }
    }
}