using System.Threading.Tasks;
using Decembrist.Events;
using Godot;

namespace Decembrist.Example.EventBusEvents
{
    public class EventTest : Node2D, ITest
    {
        public async Task Test()
        {
            var messageCount = 0;

            void IncrementCountCallback()
            {
                messageCount++;
            }
            
            var subscription1 = this.EventListener("event1", IncrementCountCallback);
            var subscription2 = this.EventListener("event1", IncrementCountCallback);
            this.FireEvent("event1");
            subscription2.Stop();
            subscription1.Stop();
            this.FireEvent("event1");
            Assertions.AssertTrue(messageCount == 2, "Event without params test");
            
            messageCount = 0;

            void PlusCountCallback(int input)
            {
                messageCount += input;
            }
            subscription1 = this.EventListener<int>("event2", PlusCountCallback);
            subscription2 = this.EventListener<int>("event2", PlusCountCallback);
            this.FireEvent("event2", 2);
            subscription2.Stop();
            subscription1.Stop();
            this.FireEvent("event2", 2);
            Assertions.AssertTrue(messageCount == 4, "Event with params test");
            GD.Print("EventBus event test stopped...........................................");
        }
    }
}
