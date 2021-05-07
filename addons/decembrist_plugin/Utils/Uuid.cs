namespace Decembrist.Utils
{
    public static class Uuid
    {
        public static string Get() => System.Guid.NewGuid().ToString();
    }
}