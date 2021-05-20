#nullable enable
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Decembrist.Di;
using Decembrist.Events;
using Decembrist.Utils;
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
        private new delegate void EmitSignal(string address, string messageId);

        [Signal]
        internal delegate void ReplySignal(string replyMessageId);

        private ConcurrentDictionary<string, Object> _messages = new();
        private ConcurrentDictionary<string, Object> _replies = new();
        private ConcurrentDictionary<string, object?> _eventMessages = new();
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
        /// <param name="eventBusRequest">reply event</param>
        /// <typeparam name="TResponse"></typeparam>
        internal void AddReplyMessage<TResponse>(EventBusRequest<TResponse> eventBusRequest)
        {
            _replies[eventBusRequest.MessageId] = eventBusRequest;
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
            Action<EventBusRequest<TResponse>> replyHandler)
        {
            var eventMessage = new ReplyEventBusRequest<TRequest, TResponse>(this, message);
            var messageId = eventMessage.MessageId;
            const string signal = nameof(ReplySignal);
            var callback = this.Subscribe(signal, (string replyMessageId) =>
            {
                if (messageId != replyMessageId) return;

                _replies.TryRemove(messageId, out var reply);
                if (reply is EventBusRequest<TResponse> response)
                {
                    replyHandler(response);
                    this.Unsubscribe(signal, _replyCallbacks[messageId]);
                }
            });
            _replyCallbacks[messageId] = callback;
            _messages[messageId] = eventMessage;
            EmitSignal(nameof(MessageSignal), to, messageId);
        }
        
        /// <summary>
        /// Send message through event bus for every registered consumer
        /// </summary>
        /// <param name="to">Message address</param>
        /// <param name="message">Message content</param>
        /// <param name="replyHandler">Consumer response handler</param>
        /// <typeparam name="TRequest">Message content type</typeparam>
        /// <typeparam name="TResponse">Response message type</typeparam>
        public void Emit(string @event, object? body)
        {
            var messageId = Uuid.Get();
            _eventMessages[messageId] = body;
            EmitSignal(nameof(EmitSignal), @event, messageId);
            _eventMessages.TryRemove(messageId, out var eventBody);
        }

        /// <summary>
        /// Async version for <see cref="Send{T,TR}(string,T?,System.Action{EventBusRequest{T}})"/>
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
        public EventBusSubscription MessageEndpoint<TRequest, TResponse>(
            string from,
            Action<ReplyEventBusRequest<TRequest, TResponse>> messageHandler)
        {
            const string signal = nameof(MessageSignal);
            var callback = this.Subscribe(signal, (string source, string messageId) =>
                {
                    if (source != from) return;

                    _messages.TryRemove(messageId, out var message);
                    if (message is not ReplyEventBusRequest<TRequest, TResponse> eventMessage) return;
                    
                    messageHandler(eventMessage);
                    if (!eventMessage.Replied)
                    {
                        eventMessage.EmptyReply();
                    }
                }
            );

            return new EventBusSubscription(this, callback, signal);
        }

        /// <summary>
        /// Subscribes on event emits
        /// </summary>
        /// <param name="event">Event name</param>
        /// <param name="eventHandler">Handler callback</param>
        /// <typeparam name="TRequest">Event body type</typeparam>
        /// <returns>Subscription</returns>
        public EventBusSubscription EventListener<TRequest>(string @event, Action<TRequest> eventHandler)
        {
            const string signal = nameof(EmitSignal);
            var callback = this.Subscribe(signal, (string address, string messageId) =>
            {
                if (@event != address) return;

                var body = _eventMessages[messageId];
                if (body is TRequest or null)
                {
                    eventHandler((TRequest) body);
                }
            });
            
            return new EventBusSubscription(this, callback, signal);
        }
    }
}