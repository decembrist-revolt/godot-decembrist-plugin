using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class GraphNodes
    {
        public const string CloseRequestSignal = "close_request";
        public const string DraggedSignal = "dragged";
        public const string OffsetChangedSignal = "offset_changed";
        public const string RaiseRequestSignal = "raise_request";
        public const string ResizeRequestSignal = "resize_request";
        public const string SlotUpdatedSignal = "slot_updated";
        
        /// <summary>
        /// Emitted when the GraphNode is requested to be closed. Happens on clicking the close button
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnCloseRequest(this GraphNode node, VoidFunc action)
        {
            var callback = node.Subscribe(CloseRequestSignal, action);
            return () => node.Unsubscribe(CloseRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the GraphNode is dragged.
        /// </summary>
        /// <param name="node">This node</param>
        /// <param name="action">action ( Vector2 from, Vector2 to )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnDragged(this GraphNode node, Action<Vector2, Vector2> action)
        {
            var callback = node.Subscribe(DraggedSignal, action);
            return () => node.Unsubscribe(DraggedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the GraphNode is moved.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnOffsetChanged(this GraphNode node, VoidFunc action)
        {
            var callback = node.Subscribe(OffsetChangedSignal, action);
            return () => node.Unsubscribe(OffsetChangedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the GraphNode is requested to be displayed over other ones.
        /// Happens on focusing (clicking into) the GraphNode.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnRaiseRequest(this GraphNode node, VoidFunc action)
        {
            var callback = node.Subscribe(RaiseRequestSignal, action);
            return () => node.Unsubscribe(RaiseRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when trying to append text that would overflow the max_length.
        /// </summary>
        /// <param name="node">This node</param>
        /// <param name="action">action ( Vector2 new_minsize )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnResizeRequest(this GraphNode node, Action<Vector2> action)
        {
            var callback = node.Subscribe(ResizeRequestSignal, action);
            return () => node.Unsubscribe(ResizeRequestSignal, callback);
        }

        /// <summary>
        /// Emitted when trying to append text that would overflow the max_length.
        /// </summary>
        /// <param name="node">This node</param>
        /// <param name="action">action ( int idx )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnSlotUpdated(this GraphNode node, Action<int> action)
        {
            var callback = node.Subscribe(SlotUpdatedSignal, action);
            return () => node.Unsubscribe(SlotUpdatedSignal, callback);
        }
    }
}