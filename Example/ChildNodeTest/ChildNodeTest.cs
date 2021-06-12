using System.Threading.Tasks;
using Decembrist.Di;
using Godot;

namespace Decembrist.Example.ChildNodeTest
{
    public class ChildNodeTest : Node2D, ITest
    {
        [ChildNode] private Node _node1;
        [ChildNode("Node2")] private Node _node2;
        [ChildNode("NodeGroup/SubNode")] private Node _subNode;

        public override void _Ready()
        {
            this.InjectAll();
        }

        public async Task Test()
        {
            Assertions.AssertNotNull(_node1);
            Assertions.AssertNotNull(_node2);
            Assertions.AssertNotNull(_subNode);
        }
    }
}