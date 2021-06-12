#nullable enable
using System;
using System.Linq;
using Decembrist.Converter;
using Decembrist.Di;
using Decembrist.Events;
using Godot;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace Decembrist.Autoload
{
    public class LanEventBus : Node
    {
        public void Emit<T>(string @event, object? payload) where T : class, ITypeSerializer 
        {
            var serializer = this.Resolve<T>() as ITypeSerializer 
                             ?? throw new Exception($"Serializer {typeof(T)} dependency not found");
            Emit(@event, payload, serializer);
            var serialized = serializer!.Serialize(payload);
            Rpc(nameof(EmitSerializableLanEvent), @event, serialized, serializer.GetType().FullName);
        }
        
        public void Emit(string @event, object? payload, ITypeSerializer serializer)  
        {
            var serialized = serializer!.Serialize(payload);
            Rpc(nameof(EmitSerializableLanEvent), @event, serialized, serializer.GetType().FullName);
        }
        
        public void Emit(string @event)
        {
            Rpc(nameof(EmitLanEvent), @event, null);
        }

        public void Emit(string @event, string? payload)
        {
            Rpc(nameof(EmitLanEvent), @event, payload);
        }
        
        public void Emit(string @event, bool? payload)
        {
            Rpc(nameof(EmitLanEvent), @event, payload);
        }
        
        public void Emit(string @event, int? payload)
        {
            Rpc(nameof(EmitLanEvent), @event, payload);
        }

        public void Emit(string @event, float? payload)
        {
            Rpc(nameof(EmitLanEvent), @event, payload);
        }

        [RemoteSync]
        private void EmitSerializableLanEvent(string @event, string? payload, string serializerType)
        {
            var type = Type.GetType(serializerType);
            var serializer = this.Resolve(type!) as ITypeSerializer;
            var @object = serializer!.Deserialize(payload);
            EmitLanEvent(@event, @object);
        }

        [RemoteSync]
        private void EmitLanEvent(string @event, object? payload)
        {
            switch (payload)
            {
                case null:
                    this.FireEvent(@event);
                    break;
                case string payloadString:
                    this.FireEvent(@event, payloadString);
                    break;
                case bool payloadBool:
                    this.FireEvent(@event, payloadBool);
                    break;
                case int payloadInt:
                    this.FireEvent(@event, payloadInt);
                    break;
                case float payloadFloat:
                    this.FireEvent(@event, payloadFloat);
                    break;
                case long payloadLong:
                    this.FireEvent(@event, payloadLong);
                    break;
                case double payloadDouble:
                    this.FireEvent(@event, payloadDouble);
                    break;
                case Object payloadGodot:
                    this.FireEvent(@event, payloadGodot);
                    break;
                case Array payloadArr:
                    this.FireEvent(@event, payloadArr.OfType<object?>().ToArray());
                    break;
                default:
                    this.FireEvent(@event, payload);
                    break;
            }
        }
    }
}