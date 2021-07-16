using System.Collections.Generic;
using System.Linq;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class Nodes
    {
        public const string ReadySignal = "ready";
        
        public static T FindNode<T>(this Node node, string mask, bool recursive = true, bool owned = true)
            where T : Node
        {
            return node.FindNode(mask, recursive, owned) as T;
        }
        
        public static T FindParent<T>(this Node node, string mask) where T : Node
        {
            return node.FindParent(mask) as T;
        }
        
        /// <summary>
        /// Returns children of specified type
        /// </summary>
        /// <returns>Children list</returns>
        public static List<T> GetChildren<T>(this Node node) where T : Node
        {
            return node.GetChildren().OfType<T>().ToList();
        }
        
        /// <summary>
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnReady(this Node button, VoidFunc action)
        {
            var callback = button.Subscribe(ReadySignal, action);
            return () => button.Unsubscribe(ReadySignal, callback);
        }
    }
}