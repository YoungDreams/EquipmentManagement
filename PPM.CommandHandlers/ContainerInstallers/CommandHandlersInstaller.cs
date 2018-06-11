using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Foundation.Messaging;

namespace PPM.CommandHandlers.ContainerInstallers
{
    public class CommandHandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().BasedOn(typeof(ICommandHandler<>)).WithServiceBase().LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn(typeof(ICommandHandler<,>)).WithServiceBase().LifestyleTransient());
        }
    }
}
