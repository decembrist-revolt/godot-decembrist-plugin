#nullable enable
using System;

namespace Decembrist.Events
{
    public class SendEventException : Exception
    {
        private readonly int? _code;
        
        public SendEventException(string? message, int? code) : base(message)
        {
            _code = code;
        }

        /// <summary>
        /// Get user defined error code
        /// </summary>
        /// <returns></returns>
        public int? GetCode() => _code;
        
        public static SendEventException WithMessage(string? message, int? code) => new(message, code);
    }
}