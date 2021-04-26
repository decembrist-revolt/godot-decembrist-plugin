namespace Decembrist.Di
{
    public enum DependencyScope
    {
        /// <summary>
        /// Creates only once for the whole application
        /// </summary>
        Singleton,
        /// <summary>
        /// Creates every time on demand
        /// </summary>
        Prototype
    }
}