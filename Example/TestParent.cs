using Decembrist.Di;
using Godot;

namespace Decembrist.Example
{
    public class TestParent : Node2D
    {
        [ChildNode] private ITest _diTest;
        [ChildNode] private ITest _eventBusMessageTest;
        [ChildNode] private ITest _eventBusEventTest;
        [ChildNode] private ITest _childNodeTest;
        [ChildNode] private ITest _lanEventBusEventTest;
        
        public override async void _Ready()
        {
            this.InjectAll();
            CheckTestsExistence();
            GD.Print("DI test......................................................");
            await _diTest.Test();
            GD.Print("DI test PASSED...............................................");
            GD.Print("EventBus message test........................................");
            await _eventBusMessageTest.Test();
            GD.Print("EventBus message test PASSED.................................");
            GD.Print("EventBus event test..........................................");
            await _eventBusEventTest.Test();
            GD.Print("EventBus event test PASSED...................................");
            GD.Print("ChildNode test...............................................");
            await _childNodeTest.Test();
            GD.Print("ChildNode test PASSED........................................");
            GD.Print("LanEventBus event test.......................................");
            await _lanEventBusEventTest.Test();
            GD.Print("LanEventBus event test PASSED................................");
            GD.Print("ALL PASSED...................................................");
        }

        public void CheckTestsExistence()
        {
            Assertions.AssertNotNull(_diTest);
            Assertions.AssertNotNull(_eventBusMessageTest);
            Assertions.AssertNotNull(_eventBusEventTest);
            Assertions.AssertNotNull(_childNodeTest);
            Assertions.AssertNotNull(_lanEventBusEventTest);
        }
    }
}
