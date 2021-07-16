using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class EditorPlugins
    {
        public const string MainScreenChangedSignal = "main_screen_changed";
        public const string ResourceSavedSignal = "resource_saved";
        public const string SceneChangedSignal = "scene_changed";
        public const string SceneClosedSignal = "scene_closed";

        /// <summary>
        /// Emitted when user changes the workspace (2D, 3D, Script, AssetLib). Also works with custom screens defined by plugins.
        /// </summary>
        /// <param name="plugin">EditorPlugin object</param>
        /// <param name="action">action(String screen_name)</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnMainScreenChanged(this EditorPlugin plugin, Action<string> action)
        {
            var callback = plugin.Subscribe(MainScreenChangedSignal, action);
            return () => plugin.Unsubscribe(MainScreenChangedSignal, callback);
        }
        
        /// <param name="plugin">EditorPlugin object</param>
        /// <param name="action">action( Resource resource )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnResourceSaved(this EditorPlugin plugin, Action<Resource> action)
        {
            var callback = plugin.Subscribe(ResourceSavedSignal, action);
            return () => plugin.Unsubscribe(ResourceSavedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the scene is changed in the editor. The argument will return the root node of the scene that
        /// has just become active. If this scene is new and empty, the argument will be null.
        /// </summary>
        /// <param name="plugin">EditorPlugin object</param>
        /// <param name="action">action( Node scene_root )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnSceneChanged(this EditorPlugin plugin, Action<Node> action)
        {
            var callback = plugin.Subscribe(SceneChangedSignal, action);
            return () => plugin.Unsubscribe(SceneChangedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when user closes a scene. The argument is file path to a closed scene.
        /// </summary>
        /// <param name="plugin">EditorPlugin object</param>
        /// <param name="action">action( String filepath )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnSceneClosed(this EditorPlugin plugin, Action<Node> action)
        {
            var callback = plugin.Subscribe(SceneClosedSignal, action);
            return () => plugin.Unsubscribe(SceneClosedSignal, callback);
        }
    }
}