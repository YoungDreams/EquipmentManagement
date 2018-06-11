using Castle.Windsor;

namespace Foundation.Core
{
    public static class ServiceLocator
    {
        public static IWindsorContainer Current { get; private set; }

        public static void SetServiceLocator(IWindsorContainer container)
        {
            Current = container;
        }
    }
}
