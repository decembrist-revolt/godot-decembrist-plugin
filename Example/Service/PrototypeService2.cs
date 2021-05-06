using Decembrist.Utils;

namespace Decembrist.Example.Service
{
    public class PrototypeService2: IService
    {
        private readonly SingletonService2 _service;
        private readonly string _randomNumber;

        public PrototypeService2(SingletonService2 service)
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