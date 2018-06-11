using Castle.Windsor;
using Foundation.Core;

namespace Foundation.Messaging
{
    public interface IEventPublisher
    {
        void Emit<TEvent>(TEvent @event);
    }

    public class AsyncEventPublisher : IEventPublisher
    {
        private readonly IWindsorContainer _container;

        public AsyncEventPublisher(IWindsorContainer container)
        {
            _container = container;
        }

        public void Emit<TEvent>(TEvent @event)
        {
            var eventHandlers = _container.ResolveAll<IEventHandler<TEvent>>();
            foreach (var handler in eventHandlers)
            {
                handler.Handle(@event);
            }
        }
    }

    public static class EventPublisher
    {
        public static void Emit<TEvent>(TEvent @event)
        {
            ServiceLocator.Current.Resolve<IEventPublisher>().Emit((dynamic)@event);
        }
    }
}
