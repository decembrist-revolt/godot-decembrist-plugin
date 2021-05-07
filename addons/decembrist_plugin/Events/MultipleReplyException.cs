using System;

namespace Decembrist.Events
{
    public class MultipleReplyException : Exception
    {
        public MultipleReplyException() : base("Multiple reply exception")
        {
        }
    }
}