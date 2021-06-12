using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class Networks
    {
        /// <summary>
        /// Emitted whenever this SceneTree's network_peer connects with a new peer. ID is the peer ID of the new peer.
        /// Clients get notified when other clients connect to the same server. Upon connecting to a server,
        /// a client also receives this signal for the server (with ID being 1).
        /// </summary>
        public static AbstractCallback OnNetworkPeerConnected(this SceneTree tree, Action<int> callback)
        {
            return tree.Subscribe("network_peer_connected", callback);
        }
        
        /// <summary>
        /// Emitted whenever this SceneTree's network_peer disconnects from a peer.
        /// Clients get notified when other clients disconnect from the same server.
        /// </summary>
        public static AbstractCallback OnNetworkPeerDisconnected(this SceneTree tree, Action<int> callback)
        {
            return tree.Subscribe("network_peer_disconnected", callback);
        }
        
        /// <summary>
        /// Emitted whenever this SceneTree's network_peer successfully connected to a server. Only emitted on clients.
        /// </summary>
        public static AbstractCallback OnConnectedToServer(this SceneTree tree, Action callback)
        {
            return tree.Subscribe("connected_to_server", callback);
        }
        
        /// <summary>
        /// Emitted whenever this SceneTree's network_peer fails to establish a connection to a server.
        /// Only emitted on clients.
        /// </summary>
        public static AbstractCallback OnConnectionFailed(this SceneTree tree, Action callback)
        {
            return tree.Subscribe("connection_failed", callback);
        }
        
        /// <summary>
        /// Emitted whenever this SceneTree's network_peer disconnected from server. Only emitted on clients.
        /// </summary>
        public static AbstractCallback OnServerDisconnected(this SceneTree tree, Action callback)
        {
            return tree.Subscribe("server_disconnected", callback);
        }
    }
}