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
        /// <para><inheritdoc cref="EventBus.Send{T,TR}(string,T?,System.Action{Decembrist.Events.EventMessage{TR}})"/></para>
        /// <para>See <see cref="EventBus.Send{T,TR}(string,T?,System.Action{Decembrist.Events.EventMessage{TR}})"/></para>
        /// </summary>
        public static void SendMessage<TRequest, TResponse>(
            this Node node,
            string to,
            TRequest? message,
            Action<EventMessage<TResponse>> replyHandler) => node.GetEventBus().Send(to, message, replyHandler);
        
        /// <summary>
        /// <inheritdoc cref="SendMessage{T,TR}"/>
        /// </summary>
        public static void SendMessage<TResponse>(
            this Node node,
            string to,
            object? message,
            Action<EventMessage<TResponse>> replyHandler
        ) => node.SendMessage<object, TResponse>(to, message, replyHandler);
        
        /// <summary>
        /// <inheritdoc cref="SendMessage{T,TR}"/>
        /// <para>With null message</para>
        /// </summary>
        public static void SendMessage<TResponse>(
            this Node node,
            string to,
            Action<EventMessage<TResponse>> replyHandler) => node.SendMessage(to, null, replyHandler);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// <para>See <see cref="EventBus.Send{T,TR}(string,T?)"/></para>
        /// </summary>
        public static Task<TResponse> SendMessageAsync<TRequest, TResponse>(
            this Node node,
            string to,
            TRequest? message = default) => node.GetEventBus().Send<TRequest, TResponse>(to, message);

        /// <summary>
        /// <para><inheritdoc cref="EventBus.Consumer{T,TR}"/></para>
        /// <para>See <see cref="EventBus.Consumer{T,TR}"/></para>
        /// </summary>
        public static EventBusSubscription Consumer<TRequest, TResponse>(
            this Node node,
            string from, 
            Action<ReplyEventMessage<TRequest, TResponse>> messageHandler) => node.GetEventBus().Consumer(from, messageHandler);
        
    }
}