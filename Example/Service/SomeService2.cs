namespace Decembrist.Example.Service
{
    public class SomeService2: IService
    {
        private readonly SomeService1 _service1;

        public SomeService2(SomeService1 service1)
        {
            _service1 = service1;
        }

        public void Run() => _service1.SayHello();
    }
}