using Godot;

namespace Decembrist.Autoload
{
    public static class AutoloadUtils
    {
        /// <summary>
        /// Get DiService autoload instance
        /// </summary>
        /// <param name="node"></param>
        /// <returns>DiService singleton instance</returns>
        public static DiService GetDiService(this Node node) =>
            node.GetNode<DecembristAutoload>("/root/DecembristAutoload").DiService;
        
        /// <summary>
        /// Get event bus singleton instance
        /// </summary>
        /// <returns>EventBus instance</returns>
        public static EventBus GetEventBus(this Node node) =>
            node.GetNode<DecembristAutoload>("/root/DecembristAutoload").EventBus;
    }
}