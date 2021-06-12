using Decembrist.Converter;
using Decembrist.Events;
using Decembrist.Utils;
using Godot;

namespace Decembrist.Example.LanEventBusEventTest
{
    public class LanEventBusEventServer : Node2D
    {
        public const int ServerPort = 9999;

        public const string LanBoolEvent = "lan-bool-event";
        public const string LanStringEvent = "lan-string-event";
        public const string LanFloatEvent = "lan-float-event";
        public const string LanIntEvent = "lan-int-event";
        public const string LanSerializedEvent = "lan-serialized-event";

        public override void _Ready()
        {
            var server = new NetworkedMultiplayerENet();
            GetTree().OnNetworkPeerConnected((id) =>
            {
                GD.Print("Peer connected");
                this.FireLanEvent(LanBoolEvent, true);
                this.FireLanEvent(LanStringEvent, "true");
                this.FireLanEvent(LanFloatEvent, 1.0f);
                this.FireLanEvent(LanIntEvent, 1);
                this.FireLanEvent(
                    LanSerializedEvent,
                    new SerializedClass {Field = "true"},
                    new SerializedClassSerializer()
                );
                GD.Print("Events fired");
            });
            server.CreateServer(ServerPort, 1);
            GetTree().NetworkPeer = server;
        }

        public class SerializedClass
        {
            public string Field;
        }

        public class SerializedClassSerializer : ITypeSerializer
        {
            public string Serialize(object @object)
            {
                return (@object as SerializedClass)?.Field;
            }

            public object Deserialize(string @object)
            {
                return new SerializedClass {Field = @object};
            }
        }
    }
}