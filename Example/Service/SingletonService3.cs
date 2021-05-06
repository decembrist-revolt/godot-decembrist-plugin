namespace Decembrist.Example.Service
{
    public class SingletonService3: IService
    {
        private readonly SingletonService2 _service2;

        public SingletonService3(SingletonService2 service2)
        {
            _service2 = service2;
        }

        public string GetString() => _service2.GetString();
    }
}