namespace Decembrist.Di.Handler.Processor
{
    public interface IDependencyPostProcessor
    {
        void Process(object instance);
    }
}