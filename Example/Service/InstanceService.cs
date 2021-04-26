namespace Decembrist.Example.Service
{
    public class InstanceService
    {
        
        public static InstanceService Instance = new InstanceService();

        private InstanceService()
        {
        }
    }
}