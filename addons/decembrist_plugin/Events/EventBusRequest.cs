using Decembrist.Autoload;
using Decembrist.Utils;
using Godot;

#nullable enable
namespace Decembrist.Events
{
    public class EventBusRequest<T> : Object
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

        protected internal EventBusRequest(T? content, string? error = null, int? errorCode = null) 
            : this(Uuid.Get(), content, error, errorCode)
        {
        }

        protected internal EventBusRequest(string messageId, T? content, string? error = null, int? errorCode = null)
        {
            Error = error;
            ErrorCode = errorCode;
            Content = content;
            MessageId = messageId;
        }

        /// <returns>False if error found</returns>
        public bool IsError() => Error != null;
    }

    public class ReplyEventBusRequest<TRequest, TResponse> : EventBusRequest<TRequest>
    {
        private readonly EventBus _eventBus;

        public bool Replied { get; private set; }

        internal ReplyEventBusRequest(EventBus eventBus, TRequest? content) : base(content)
        {
            _eventBus = eventBus;
        }

        /// <summary>
        /// Reply to sender
        /// </summary>
        /// <param name="content">Response content</param>
        /// <exception cref="MultipleReplyException">If already replied</exception>
        public void Reply(TResponse? content)
        {
            if (Replied) throw new MultipleReplyException();
            _eventBus.AddReplyMessage(new EventBusRequest<TResponse>(MessageId, content));
            _eventBus.EmitSignal(nameof(EventBus.ReplySignal), MessageId);
            Replied = true;
        }

        /// <summary>
        /// Reply null to sender
        /// </summary>
        /// <exception cref="MultipleReplyException">If already replied</exception>
        public void EmptyReply() => Reply(default);

        /// <summary>
        /// Reply to sender with error
        /// </summary>
        /// <param name="error">Error text</param>
        /// <param name="code">Error code</param>
        /// <exception cref="MultipleReplyException">If already replied</exception>
        public void ErrorReply(string error, int? code = null)
        {
            if (Replied) throw new MultipleReplyException();
            _eventBus.AddReplyMessage(new EventBusRequest<TResponse>(MessageId, default, error, code));
            _eventBus.EmitSignal(nameof(EventBus.ReplySignal), MessageId);
            Replied = true;
        }
    }
}