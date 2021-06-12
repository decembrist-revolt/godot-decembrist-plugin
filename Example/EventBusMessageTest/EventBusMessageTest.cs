using System.Threading.Tasks;
using Decembrist.Di;
using Godot;

namespace Decembrist.Example.EventBusMessageTest
{
    public class EventBusMessageTest : Node2D, ITest
    {
        public const string ConsumerAddress1 = "consumer-address1";
        public const string ConsumerAddress2 = "consumer-address2";
        public const string TestError = "test error";
        public const int TestErrorCode = 2;

        [ChildNode] private ITest _producer;
        [ChildNode] private ITest _consumer;

        public override void _Ready()
        {
            this.InjectAll();
        }

        public async Task Test()
        {
            await _consumer.Test();
            await _producer.Test();
        }
    }
}
