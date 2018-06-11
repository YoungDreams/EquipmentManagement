using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Foundation.Core;

namespace Foundation.Windsor
{
    public static class ComponentRegistrationExtensions
    {
        public static ComponentRegistration<TService> SupportProvider<TService>(this ComponentRegistration<TService> registration, IWindsorContainer container) where TService : class
        {
            container.Register(
                Component.For<IProvider<TService>>()
                    .ImplementedBy<WindsorProvider<TService>>()
                    .LifestyleSingleton());
            return registration;
        }
    }
}
