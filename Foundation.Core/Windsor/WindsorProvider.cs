using Castle.Windsor;
using Foundation.Core;

namespace Foundation.Windsor
{
    public class WindsorProvider<T> : IProvider<T>
    {
        private readonly IWindsorContainer _container;

        public WindsorProvider(IWindsorContainer container)
        {
            _container = container;
        }
        public T Get()
        {
            return _container.Resolve<T>();
        }
    }
}