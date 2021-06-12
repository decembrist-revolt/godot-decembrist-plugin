using System;
using System.Linq;
using System.Threading.Tasks;
using Decembrist.Events;
using Decembrist.Utils.Task;
using Godot;
using static Decembrist.Example.LanEventBusEventTest.LanEventBusEventServer;

namespace Decembrist.Example.LanEventBusEventTest
{
    public class LanEventBusEventTest : Node2D, ITest
    {
        public override void _Ready()
        {
        }

        public async Task Test()
        {
            var server = new NetworkedMultiplayerENet();
            var boolEvent = AwaitResponse(LanBoolEvent, (bool payload) => payload);
            var stringEvent = AwaitResponse(LanStringEvent, (string payload) => payload == "true");
            var intEvent = AwaitResponse(LanIntEvent, (int payload) => payload == 1);
            var floatEvent = AwaitResponse(LanFloatEvent, (float payload) => payload == 1.0f);
            var serializedEvent =
                AwaitResponse(LanSerializedEvent, (SerializedClass payload) => payload.Field == "true");
            var error = server.CreateClient("127.0.0.1", ServerPort);
            Assertions.AssertTrue(error == Error.Ok, "Connection without errors");
            GetTree().NetworkPeer = server;
            var results = await Task.WhenAll(
                boolEvent, stringEvent, intEvent, floatEvent, serializedEvent);
            Assertions.AssertTrue(results.All(result => result), "All Lan events done");
        }

        private async Task<bool> AwaitResponse<TPayload>(string @event, Func<TPayload, bool> payloadCallback)
        {
            return await Promises.Of<bool>((resolve) =>
            {
                this.EventListener<TPayload>(@event, (payload) =>
                {
                    GD.Print($"Lan event {@event} received");
                    resolve(payloadCallback(payload));
                });
                GD.Print($"Waiting for lan event {@event}");
            });
        }
    }
}