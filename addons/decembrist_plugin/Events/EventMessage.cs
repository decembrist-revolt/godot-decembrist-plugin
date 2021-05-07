using System;
using Decembrist.Autoload;
using Decembrist.Utils;
using Decembrist.Utils.Callback;
using Godot;
using Object = Godot.Object;

#nullable enable
namespace Decembrist.Events
{
    public class EventMessage<T> : Object
    {
        /// <summary>
        /// Event error
        /// </summary>
        public readonly string? Error;

        /// <summary>
        /// User defined error code
        /// </summary>
        public readonly int? ErrorCode;

        /// <summary>
        /// Event message content
        /// </summary>
        public readonly T? Content;
        
        /// <summary>
        /// Uniq message identifier
        /// </summary>
        public readonly string MessageId;

        protected internal EventMessage(T? content, string? error = null, int? errorCode = null) 
            : this(Uuid.Get(), content, error, errorCode)
        {
        }

        protected internal EventMessage(string messageId, T? content, string? error = null, int? errorCode = null)
        {
            Error = error;
            ErrorCode = errorCode;
            Content = content;
            MessageId = messageId;
        }

        /// <returns>False if error found</returns>
        public bool IsError() => Error != null;
    }

    public class ReplyEventMessage<TRequest, TResponse> : EventMessage<TRequest>
    {
        private readonly EventBus _eventBus;

        private bool _replied = false;

        internal ReplyEventMessage(EventBus eventBus, TRequest? content) : base(content)
        {
            _eventBus = eventBus;
        }

        /// <summary>
        /// Reply sender
        /// </summary>
        /// <param name="content">Response content</param>
        /// <exception cref="MultipleReplyException">If already replied</exception>
        public void Reply(TResponse content)
        {
            if (_replied) throw new MultipleReplyException();
            _eventBus.AddReplyMessage(new EventMessage<TResponse>(MessageId, content));
            _eventBus.EmitSignal(nameof(EventBus.ReplySignal), MessageId);
            _replied = true;
        }

        /// <summary>
        /// Reply sender with error
        /// </summary>
        /// <param name="error">Error text</param>
        /// <param name="code">Error code</param>
        /// <exception cref="MultipleReplyException">If already replied</exception>
        public void ErrorReply(string error, int? code = null)
        {
            if (_replied) throw new MultipleReplyException();
            _eventBus.AddReplyMessage(new EventMessage<TResponse>(MessageId, default, error, code));
            _eventBus.EmitSignal(nameof(EventBus.ReplySignal), MessageId);
            _replied = true;
        }
    }
}