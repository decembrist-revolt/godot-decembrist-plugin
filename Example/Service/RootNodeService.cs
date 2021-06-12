using Decembrist.Di;
using Godot;

namespace Decembrist.Example.Service
{
    public class RootNodeService
    {
        public Node RootNode;
        
        public RootNodeService([RootNode] Node rootNode1, [RootNode] Viewport rootNode2)
        {
            Assertions.AssertTrue(rootNode1 == rootNode2, "Multiple root node inject test");
            RootNode = rootNode1;
        }
    }
}