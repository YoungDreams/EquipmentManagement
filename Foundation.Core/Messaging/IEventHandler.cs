namespace Foundation.Messaging
{
    public interface IEventHandler<in TEvent>
    {
        void Handle(TEvent evt);
    }

    public interface IAsyncEventHandler<in TEvent>
    {
        void Handle(TEvent evt);
    }
}