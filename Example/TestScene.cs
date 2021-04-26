using System;
using Godot;
using Decembrist.Di;
using Decembrist.Example.Service;
using Decembrist.Utils;

namespace Decembrist.Example
{
    public class TestScene : Node2D
    {
        [Inject]
        private IService service1;
        
        [Inject]
        private IService service2;
        
        [Inject]
        private PrototypeService1 prototypeService1;
        
        [Inject]
        private PrototypeService1 prototypeService2;
        
        [Inject]
        private InstanceService instanceService;
        
        public override void _Ready()
        {
            // init dependencies
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            this.InjectAll();
            watch.Stop();
            GD.Print($"Injection time Time: {watch.ElapsedMilliseconds} ms");
            GD.Print("Service assertions................................................");
            AssertTrue(service1 != null, "singleton service exists");
            AssertTrue(this.Resolve<IService>() != null, "singleton service exists");
            AssertTrue(instanceService == InstanceService.Instance, "service is singleton");
            AssertTrue(this.Resolve<IService>() == service1, "service is singleton");
            AssertTrue(service1 == service2, "service is singleton");
            AssertTrue(this.Resolve<PrototypeService1>() != null, "prototype service exists");
            AssertTrue(this.Resolve<PrototypeService1>() != prototypeService1, "service is prototype");
            AssertTrue(prototypeService1 != prototypeService2, "service is prototype");
            GD.Print("Service test......................................................");
            ServiceEcho();
        }

        private void ServiceEcho()
        {
            GD.Print($"singleton1 says              {service1.GetString()}");   
            GD.Print($"singleton2 says              {service2.GetString()}");   
            GD.Print($"resolved singleton says      {this.Resolve<IService>().GetString()}");   
            GD.Print($"prototype1 says              {prototypeService1.GetString()}");   
            GD.Print($"prototype2 says              {prototypeService2.GetString()}");   
            GD.Print($"resolved prototype says      {this.Resolve<PrototypeService1>().GetString()}");   
        }

        private static void AssertTrue(bool expression, string test)
        {
            if (expression)
            {
                GD.Print($"PASSED:{test}");
            }
            else
            {
                var message = $"FAILED:{test}";
                GD.PrintErr(message);
                throw new Exception(message);
            }
        }
        
    }
}
