using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Foundation.Messaging;

namespace Foundation.ContainerInstallers
{
    public class MessagingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventPublisher>().ImplementedBy<AsyncEventPublisher>().LifestyleSingleton());
            container.Register(Component.For<ICommandService>().ImplementedBy<CommandService>().LifestyleSingleton());
        }
    }
}
