using System.Collections.Generic;
using System.Threading.Tasks;
using Decembrist.Events;
using Godot;
using static Decembrist.Example.Assertions;

namespace Decembrist.Example.EventBusMessageTest
{
    [Tool]
    public class Producer : Node2D, ITest
    {
        private readonly List<int> _responses = new();

        private string _errorMessage;

        private int? _errorCode;

        [Export] private string _messageAddress;

        public async Task Test()
        {
            const int messageCount = 6;
            ProduceMessages(messageCount);
            await Task.Delay(1000);
            AssertTrue(_responses.Count == 5, "all messages consumed");
            for (var i = 1; i < messageCount - 1; i++)
            {
                AssertTrue(_responses[i - 1] == i + 1, "message response ok");
            }

            AssertTrue(_errorMessage == EventBusMessageTest.TestError, "event bus error message ok");
            AssertTrue(_errorCode == EventBusMessageTest.TestErrorCode, "event bus error code ok");
        }

        private async void ProduceMessages(int messageCount)
        {
            for (var i = 1; i <= messageCount; i++)
            {
                try
                {
                    var response = await this.SendMessageRequestAsync<int, int>(_messageAddress, i);
                    _responses.Add(response);
                }
                catch (SendEventException ex)
                {
                    _errorMessage = ex.Message;
                    _errorCode = ex.GetCode();
                }
            }
        }
    }
}