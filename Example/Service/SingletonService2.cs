namespace Decembrist.Example.Service
{
    public class SingletonService2: IService
    {
        private readonly SingletonService1 _service1;

        public SingletonService2(SingletonService1 service1)
        {
            _service1 = service1;
        }

        public string GetString() => _service1.SayHello();
    }
}