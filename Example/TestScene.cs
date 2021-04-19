using Godot;
using Decembrist.Di;
using Decembrist.Example.Service;
using Decembrist.Utils;

namespace Decembrist.Example
{
    public class TestScene : Node2D
    {
        [Inject]
        private IService service;
        
        public override void _Ready()
        {
            this.InjectAll();
            service.Run();
        }
        
    }
}
