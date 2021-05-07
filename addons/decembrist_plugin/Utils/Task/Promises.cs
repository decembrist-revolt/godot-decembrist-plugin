#nullable enable
using System;
using System.Threading.Tasks;
using Godot;
using VoidTask = System.Threading.Tasks.Task;

namespace Decembrist.Utils.Task
{
    public static class Promises
    {
        public static Task<T> Of<T>(Action<Action<T?>, Action<Exception>> block) => new Promise<T>(block).Start();
        
        public static VoidTask Of(Action<Action<object?>, Action<Exception>> block) => new Promise<object>(block).Start();
    }
}