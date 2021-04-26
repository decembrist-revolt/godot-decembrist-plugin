namespace Decembrist.Di
{
    /// <summary>
    /// Implement interface to register dependencies
    /// To edit class name use Decembrist/Config Class property
    /// </summary>
    public interface IDecembristConfiguration
    {
        public ContainerBuilder ConfigDi(ContainerBuilder builder);
    }
}