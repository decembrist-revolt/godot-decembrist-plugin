using Decembrist.Utils;

namespace Decembrist.Example.Service
{
    public class PrototypeService1: IService
    {
        public readonly PrototypeService2 PrototypeService;
        
        private readonly SingletonService2 _service;
        private readonly string _randomNumber;

        public PrototypeService1(SingletonService2 service, PrototypeService2 prototypeService)
        {
            _service = service;
            PrototypeService = prototypeService;
            _randomNumber = Random.RandomInt(1, 100).ToString();
        }

        public string GetString()
        {
            return $"{_randomNumber} {_service.GetString()}";
        }
    }
}