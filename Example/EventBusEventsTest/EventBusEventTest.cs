using System.Threading.Tasks;
using Decembrist.Di;
using Godot;

namespace Decembrist.Example.EventBusEvents
{
    public class EventBusEventTest : Node2D, ITest
    {
        [ChildNode] private ITest _eventTest;
        
        public override void _Ready()
        {
            this.InjectAll();
        }

        public async Task Test()
        {
            await _eventTest.Test();
        }
    }
}
