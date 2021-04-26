using Decembrist.Utils;

namespace Decembrist.Example.Service
{
    public class PrototypeService1: IService
    {
        private readonly SingletonService2 _service;
        private readonly string _randomNumber;

        public PrototypeService1(SingletonService2 service)
        {
            _service = service;
            _randomNumber = Random.RandomInt(1, 100).ToString();
        }

        public string GetString()
        {
            return $"{_randomNumber} {_service.GetString()}";
        }
    }
}