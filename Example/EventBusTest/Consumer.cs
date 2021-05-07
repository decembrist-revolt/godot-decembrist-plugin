using Decembrist.Events;
using Godot;

namespace Decembrist.Example.EventBusTest
{
    public class Consumer : Node2D
    {
        public const string ConsumerAddress1 = "consumer-address1";
        public const string ConsumerAddress2 = "consumer-address2";
        public const string TestError = "test error";
        public const int TestErrorCode = 2;

        private EventBusSubscription _subscription1;
        private EventBusSubscription _subscription2;
    
        public override void _Ready()
        {
            var messageCount1 = 0;
            _subscription1 = this.Consumer<int, int>(ConsumerAddress1, message =>
            {
                messageCount1++;
                HandleMessage(_subscription1, message, messageCount1);
            });
            var messageCount2 = 0;
            _subscription2 = this.Consumer<int, int>(ConsumerAddress2, message =>
            {
                messageCount2++;
                HandleMessage(_subscription2, message, messageCount2);
            });
        }

        private void HandleMessage(
            EventBusSubscription eventBusSubscription, 
            ReplyEventMessage<int, int> message,
            int messageCount)
        {
            Assertions.AssertTrue(!message.IsError(), "not error message");
            if (messageCount > 5)
            {
                message.ErrorReply("test error", 2);
                eventBusSubscription.Stop();
            }
            else
            {
                message.Reply(message.Content + 1);
            }
        }
    }
}
