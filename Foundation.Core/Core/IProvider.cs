namespace Foundation.Core
{
    public interface IProvider<out T>
    {
        T Get();
    }
}
