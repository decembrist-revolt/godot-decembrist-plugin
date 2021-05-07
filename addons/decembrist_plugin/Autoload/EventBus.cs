#nullable enable
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Decembrist.Di;
using Decembrist.Events;
using Decembrist.Utils.Callback;
using Decembrist.Utils.Task;
using Godot;
using Object = Godot.Object;

namespace Decembrist.Autoload
{
    public class EventBus : Node
    {
        public const string NodeName = "EventBus";

        [Signal]
        private delegate void MessageSignal(string source, string messageId);

        [Signal]
        internal delegate void ReplySignal(string replyMessageId);

        private ConcurrentDictionary<string, Object> _messages = new();
        private ConcurrentDictionary<string, Object> _replies = new();
        private ConcurrentDictionary<string, AbstractCallback> _replyCallbacks = new();

        public EventBus()
        {
            Name = NodeName;
        }

        public override void _Ready()
        {
            this.InjectAll();
        }

        /// <summary>
        /// Add reply event to list
        /// </summary>
        /// <param name="eventMessage">reply event</param>
        /// <typeparam name="TResponse"></typeparam>
        internal void AddReplyMessage<TResponse>(EventMessage<TResponse> eventMessage)
        {
            _replies[eventMessage.MessageId] = eventMessage;
        }

        /// <summary>
        /// Send message through event bus for every registered consumer
        /// </summary>
        /// <param name="to">Message address</param>
        /// <param name="message">Message content</param>
        /// <param name="replyHandler">Consumer response handler</param>
        /// <typeparam name="TRequest">Message content type</typeparam>
        /// <typeparam name="TResponse">Response message type</typeparam>
        public void Send<TRequest, TResponse>(string to, TRequest? message,
            Action<EventMessage<TResponse>> replyHandler)
        {
            var eventMessage = new ReplyEventMessage<TRequest, TResponse>(this, message);
            var messageId = eventMessage.MessageId;
            var callback = this.Subscribe(nameof(ReplySignal), (string replyMessageId) =>
            {
                if (messageId != replyMessageId) return;

                _replies.TryRemove(messageId, out var reply);
                if (reply is EventMessage<TResponse> response)
                {
                    replyHandler(response);
                    this.Unsubscribe(nameof(ReplySignal), _replyCallbacks[messageId]);
                }
            });
            _replyCallbacks[messageId] = callback;
            _messages[messageId] = eventMessage;
            EmitSignal(nameof(MessageSignal), to, messageId);
        }

        /// <summary>
        /// Async version for <see cref="Send{T,TR}(string,T?,System.Action{Decembrist.Events.EventMessage{TR}})"/>
        /// </summary>
        /// <param name="to">Message address</param>
        /// <param name="message">Message content</param>
        /// <typeparam name="TRequest">Message content type</typeparam>
        /// <typeparam name="TResponse">Response message type</typeparam>
        /// <returns>Consumer response task</returns>
        public Task<TResponse> Send<TRequest, TResponse>(string to, TRequest? message = default)
        {
            return Promises.Of<TResponse>((resolve, reject) =>
            {
                Send<TRequest, TResponse>(to, message, (responseMessage) =>
                {
                    if (responseMessage.IsError())
                    {
                        reject(new SendEventException(responseMessage.Error, responseMessage.ErrorCode));
                    }
                    else
                    {
                        resolve(responseMessage.Content);
                    }
                });
            });
        }

        /// <summary>
        /// Subscribe on messages from "<paramref name="from"/>" address
        /// </summary>
        /// <param name="from">Message address</param>
        /// <param name="messageHandler">Message handler</param>
        /// <typeparam name="TRequest">Message content type</typeparam>
        /// <typeparam name="TResponse">Response message type</typeparam>
        /// <returns>Subscription</returns>
        public EventBusSubscription Consumer<TRequest, TResponse>(
            string from,
            Action<ReplyEventMessage<TRequest, TResponse>> messageHandler)
        {
            const string signal = nameof(MessageSignal);
            var callback = this.Subscribe(signal, (string source, string messageId) =>
                {
                    if (source != from) return;

                    _messages.TryRemove(messageId, out var message);
                    if (message is ReplyEventMessage<TRequest, TResponse> eventMessage)
                    {
                        messageHandler(eventMessage);
                    }
                }
            );

            return new EventBusSubscription(this, callback, signal);
        }
    }
}