#nullable enable
using System;
using System.Threading.Tasks;
using VoidTask = System.Threading.Tasks.Task;

namespace Decembrist.Utils.Task
{
    public static class Promises
    {
        public static Task<T> Of<T>(Action<Action<T?>, Action<Exception>> block) => new Promise<T>(block).Start();

        public static Task<T> Of<T>(Action<Action<T?>> block) => new Promise<T>((resolve, _) => block(resolve)).Start();

        public static VoidTask Of(Action<Action<object?>> block) =>
            new Promise<object?>((resolve, _) => block(resolve)).Start();

        public static VoidTask Of(Action<Action<object?>, Action<Exception>> block) =>
            new Promise<object>(block).Start();
    }
}