using System;
using Decembrist.Di;
using Godot;

namespace Decembrist.Autoload
{
    public class DecembristAutoload: Node
    {
        [SettingsValue(DecembristSettings.EventBusEnabled)]
        private bool _eventBusEnabled;
        
        [SettingsValue(DecembristSettings.LanEventsEnabled)]
        private bool _lanEventBusEnabled;
        
        public readonly DiService DiService;
        private EventBus _eventBus;
        private LanEventBus _lanEventBus;

        public EventBus EventBus
        {
            get
            {
                CheckEventBus();
                return _eventBus;
            }
        }

        public LanEventBus LanEventBus
        {
            get
            {
                CheckEventBus();
                CheckLanEventBus();
                return _lanEventBus;
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
            if (_eventBusEnabled)
            {
                _eventBus = DiService.Resolve<EventBus>();
                AddChild(_eventBus);
            }

            if (_lanEventBusEnabled)
            {
                _lanEventBus = DiService.Resolve<LanEventBus>();
                AddChild(_lanEventBus);
            }
            
            DiService.HandlePostProcessors();
        }

        private void CheckEventBus()
        {
            if (!_eventBusEnabled)
            {
                throw new Exception($"Event bus disabled. Check {DecembristSettings.EventBusEnabled}");
            }
        }

        private void CheckLanEventBus()
        {
            if (!_lanEventBusEnabled)
            {
                throw new Exception($"Event bus disabled. Check {DecembristSettings.LanEventsEnabled}");
            }
        }
    }
}