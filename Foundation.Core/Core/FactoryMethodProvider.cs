using System;
using Castle.Windsor;

namespace Foundation.Core
{
    public class FactoryMethodProvider<T> : IProvider<T>
    {
        private readonly Func<T> _factoryMethod;

        public FactoryMethodProvider(Func<T> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        public T Get()
        {
            return _factoryMethod();
        }
    }

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
