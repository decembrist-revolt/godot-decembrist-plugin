#nullable enable
using System;
using System.Threading.Tasks;
using Decembrist.Autoload;
using Godot;

namespace Decembrist.Events
{
    public static class EventBusUtils
    {
        /// <summary>
        /// <para><inheritdoc cref="EventBus.MessageEndpoint{TRequest,TResponse}"/></para>
        /// <para>See <see cref="EventBus.MessageEndpoint{TRequest,TResponse}"/></para>
        /// </summary>
        public static EventBusSubscription MessageEndpoint<TRequest, TResponse>(
            this Node node,
            string from,
            Action<ReplyEventBusRequest<TRequest, TResponse>> messageHandler) =>
            node.GetEventBus().MessageEndpoint(from, messageHandler);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Send{TRequest,TResponse}(string,TRequest?)"/></para>
        /// <para>See <see cref="EventBus.Send{TRequest,TResponse}(string,TRequest?)"/></para>
        /// </summary>
        public static void SendMessageRequest<TRequest, TResponse>(
            this Node node,
            string to,
            TRequest? message,
            Action<EventBusRequest<TResponse>> replyHandler) => node.GetEventBus().Send(to, message, replyHandler);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.MessageEndpoint{TRequest,TResponse}"/></para>
        /// <para>See <see cref="EventBus.MessageEndpoint{TRequest,TResponse}"/></para>
        /// </summary>
        public static EventBusSubscription MessageListener<TRequest>(
            this Node node,
            string from,
            Action<TRequest?> messageHandler)
        {
            void MessageHandlerCallback(ReplyEventBusRequest<TRequest, object?> message)
            {
                messageHandler(message.Content);
                message.EmptyReply();
            }

            return node.GetEventBus().MessageEndpoint<TRequest, object?>(from, MessageHandlerCallback);
        }

        /// <summary>
        /// <inheritdoc cref="SendMessageRequest{TRequest,TResponse}"/>
        /// </summary>
        public static void SendMessageAndForget<TRequest>(
            this Node node,
            string to,
            TRequest? message) => node.SendMessageRequest<TRequest, object?>(to, message, _ => { });

        /// <summary>
        /// <para><inheritdoc cref="EventBus.MessageEndpoint{TRequest,TResponse}"/></para>
        /// <para>See <see cref="EventBus.MessageEndpoint{TRequest,TResponse}"/></para>
        /// </summary>
        public static EventBusSubscription NotificationListener(this Node node, string from, Action notificationHandler)
        {
            return node.GetEventBus().MessageListener<object?>(from, _ => notificationHandler());
        }

        /// <summary>
        /// <inheritdoc cref="SendMessageRequest{TRequest,TResponse}"/>
        /// <para>With null message</para>
        /// </summary>
        public static void SendNotification(
            this Node node,
            string to) => node.SendMessageAndForget<object?>(to, null);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// <para>See <see cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// </summary>
        public static Task<TResponse> SendMessageRequestAsync<TRequest, TResponse>(
            this Node node,
            string to,
            TRequest? message) => node.GetEventBus().Send<TRequest, TResponse>(to, message);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// <para>See <see cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// </summary>
        public static async Task SendMessageAndForgetAsync<TRequest>(
            this Node node,
            string to,
            TRequest? message) => await SendMessageRequestAsync<TRequest?, object?>(node, to, message);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// <para>See <see cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// </summary>
        public static async Task SendNotificationAsync(this Node node, string to) =>
            await SendMessageAndForgetAsync<object?>(node, to, null);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Emit"/></para>
        /// <para>See <see cref="EventBus.Emit"/></para>
        /// </summary>
        public static void FireEvent<TBody>(this Node node, string @event, TBody body) =>
            node.GetEventBus().Emit(@event, body);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.EventListener{TRequest}"/></para>
        /// <para>See <see cref="EventBus.EventListener{TRequest}"/></para>
        /// </summary>
        public static EventBusSubscription EventListener<TBody>(
            this Node node,
            string @event,
            Action<TBody> eventHandler) => node.GetEventBus().EventListener(@event, eventHandler);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Emit"/></para>
        /// <para>See <see cref="EventBus.Emit"/></para>
        /// </summary>
        public static void FireEvent(this Node node, string @event) => node.GetEventBus().Emit(@event, null);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.EventListener{TRequest}"/></para>
        /// <para>See <see cref="EventBus.EventListener{TRequest}"/></para>
        /// </summary>
        public static EventBusSubscription EventListener(
            this Node node,
            string @event,
            Action eventHandler) => node.GetEventBus().EventListener<object?>(@event, _ => { eventHandler(); });
    }
}