using System;
using Decembrist.Di;
using Godot;

namespace Decembrist.Autoload
{
    public class DecembristAutoload: Node
    {
        [SettingsValue(DecembristSettings.EventBusEnabled)]
        private bool _eventBusEnabled;
        
        public readonly DiService DiService;
        private EventBus _eventBus;

        public EventBus EventBus
        {
            get
            {
                if (!_eventBusEnabled)
                {
                    throw new Exception("Event bus disabled");
                }
                return _eventBus;
            }
        }

        public DecembristAutoload()
        {
            DiService = new DiService();
            AddChild(DiService);
        }

        public override void _Ready()
        {
            this.InjectAll();
            _eventBus = DiService.Resolve<EventBus>();
            AddChild(_eventBus);
        }
    }
}