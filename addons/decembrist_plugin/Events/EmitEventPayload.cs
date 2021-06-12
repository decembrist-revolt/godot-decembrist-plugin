#nullable enable
namespace Decembrist.Events
{
    public struct EmitEventPayload
    {
        public readonly string Event;
        public readonly object? Payload;

        public EmitEventPayload(string @event, object? payload)
        {
            Event = @event;
            Payload = payload;
        }
    }
}