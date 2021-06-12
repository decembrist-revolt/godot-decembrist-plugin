namespace Decembrist.Converter
{
    public interface ITypeSerializer
    {
        string Serialize(object @object);

        object Deserialize(string @object);
    }
}