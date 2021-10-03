using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class GraphEdits
    {
        public const string BeginNodeMoveSignal = "_begin_node_move";
        public const string EndNodeMoveSignal = "_end_node_move";
        public const string ConnectionFromEmptySignal = "connection_from_empty";
        public const string ConnectionRequestSignal = "connection_request";
        public const string ConnectionToEmptySignal = "connection_to_empty";
        public const string CopyNodesRequestSignal = "copy_nodes_request";
        public const string DeleteNodesRequestSignal = "delete_nodes_request";
        public const string DisconnectionRequestSignal = "disconnection_request";
        public const string DuplicateNodesRequestSignal = "duplicate_nodes_request";
        public const string NodeSelectedSignal = "node_selected";
        public const string NodeUnselectedSignal = "node_unselected";
        public const string PasteNodesRequestSignal = "paste_nodes_request";
        public const string PopupRequestSignal = "popup_request";
        public const string ScrollOffsetChangedSignal = "scroll_offset_changed";

        /// <summary>
        /// Emitted at the beginning of a GraphNode movement.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnBeginNodeMove(this GraphEdit edit, VoidFunc action)
        {
            var callback = edit.Subscribe(BeginNodeMoveSignal, action);
            return () => edit.Unsubscribe(BeginNodeMoveSignal, callback);
        }
        
        /// <summary>
        /// Emitted at the end of a GraphNode movement.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnEndNodeMove(this GraphEdit edit, VoidFunc action)
        {
            var callback = edit.Subscribe(EndNodeMoveSignal, action);
            return () => edit.Unsubscribe(EndNodeMoveSignal, callback);
        }

        /// <summary>
        /// Emitted when user dragging connection from input port into empty space of the graph.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( String to, int to_slot, Vector2 release_position )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnConnectionFromEmpty(this GraphEdit edit, Action<string, int, Vector2> action)
        {
            var callback = edit.Subscribe(ConnectionFromEmptySignal, action);
            return () => edit.Unsubscribe(ConnectionFromEmptySignal, callback);
        }
        
        /// <summary>
        /// Emitted to the GraphEdit when the connection between the from_slot slot of the from GraphNode
        /// and the to_slot slot of the to GraphNode is attempted to be created.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( String from, int from_slot, String to, int to_slot )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnConnectionRequest(this GraphEdit edit, Action<string, int, string, int> action)
        {
            var callback = edit.Subscribe(ConnectionRequestSignal, action);
            return () => edit.Unsubscribe(ConnectionRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when user dragging connection from output port into empty space of the graph.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( String from, int from_slot, Vector2 release_position )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnConnectionToEmpty(this GraphEdit edit, Action<string, int, Vector2> action)
        {
            var callback = edit.Subscribe(ConnectionToEmptySignal, action);
            return () => edit.Unsubscribe(ConnectionToEmptySignal, callback);
        }
        
        /// <summary>
        /// Emitted when the user presses Ctrl + C.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnCopyNodesRequest(this GraphEdit edit, Action action)
        {
            var callback = edit.Subscribe(CopyNodesRequestSignal, action);
            return () => edit.Unsubscribe(CopyNodesRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when a GraphNode is attempted to be removed from the GraphEdit.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnDeleteNodesRequest(this GraphEdit edit, Action action)
        {
            var callback = edit.Subscribe(DeleteNodesRequestSignal, action);
            return () => edit.Unsubscribe(DeleteNodesRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted to the GraphEdit when the connection
        /// between from_slot slot of from GraphNode and to_slot slot of to GraphNode is attempted to be removed.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( String from, int from_slot, String to, int to_slot )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnDisconnectionRequest(this GraphEdit edit, Action<string, int, string, int> action)
        {
            var callback = edit.Subscribe(DisconnectionRequestSignal, action);
            return () => edit.Unsubscribe(DisconnectionRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when a GraphNode is attempted to be duplicated in the GraphEdit.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnDuplicateNodesRequest(this GraphEdit edit, Action action)
        {
            var callback = edit.Subscribe(DuplicateNodesRequestSignal, action);
            return () => edit.Unsubscribe(DuplicateNodesRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when a GraphNode is selected.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( Node node )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnNodeSelected(this GraphEdit edit, Action<Node> action)
        {
            var callback = edit.Subscribe(NodeSelectedSignal, action);
            return () => edit.Unsubscribe(NodeSelectedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when a GraphNode is unselected.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( Node node )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnNodeUnselected(this GraphEdit edit, Action<Node> action)
        {
            var callback = edit.Subscribe(NodeUnselectedSignal, action);
            return () => edit.Unsubscribe(NodeUnselectedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the user presses Ctrl + V.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnPasteNodesRequest(this GraphEdit edit, Action action)
        {
            var callback = edit.Subscribe(PasteNodesRequestSignal, action);
            return () => edit.Unsubscribe(PasteNodesRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when a popup is requested. Happens on right-clicking in the GraphEdit.
        /// position is the position of the mouse pointer when the signal is sent.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( Vector2 position )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnPopupRequest(this GraphEdit edit, Action<Vector2> action)
        {
            var callback = edit.Subscribe(PopupRequestSignal, action);
            return () => edit.Unsubscribe(PopupRequestSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the scroll offset is changed by the user. It will not be emitted when changed in code.
        /// </summary>
        /// <param name="edit">This GraphEdit</param>
        /// <param name="action">action ( Vector2 ofs )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnScrollOffsetChanged(this GraphEdit edit, Action<Vector2> action)
        {
            var callback = edit.Subscribe(ScrollOffsetChangedSignal, action);
            return () => edit.Unsubscribe(ScrollOffsetChangedSignal, callback);
        }
    }
}