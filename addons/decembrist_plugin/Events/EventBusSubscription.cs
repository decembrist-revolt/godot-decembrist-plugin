using System;
using Decembrist.Autoload;
using Decembrist.Utils.Callback;

namespace Decembrist.Events
{
    public class EventBusSubscription
    {
        /// <summary>
        /// Unsubscribe this consumer
        /// </summary>
        public readonly Action Stop;

        public EventBusSubscription(EventBus eventBus, AbstractCallback callback, string signal)
        {
            Stop = () => eventBus.Unsubscribe(signal, callback);
        }
    }
}