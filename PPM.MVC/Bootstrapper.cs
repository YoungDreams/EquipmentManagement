using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Releasers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Foundation.Core;
using Foundation.Messaging;
using PPM.CommandHandlers.ContainerInstallers;
using PPM.Converters.ContainerInstallers;
using PPM.Entities.ContainerInstallers;
using PPM.Query.ContainerInstallers;
using PPM.Workflows.ContainerInstallers;
using PPM.Shared;

namespace PPM.MVC
{
    internal static class Bootstrapper
    {
        internal static void Bootstrap(IWindsorContainer container)
        {
#pragma warning disable 618
            container.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
#pragma warning restore 618

            container.Register(Component.For<ICommandService>().ImplementedBy<CommandService>().LifestyleSingleton());
            container.Register(Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton());

            container.Install(
                FromAssembly.This(),
                FromAssembly.Containing<CommandHandlersInstaller>(),
                FromAssembly.Containing<NHibernateConfigurationInstaller>(),
                FromAssembly.Containing<Foundation.Data.ContainerInstallers.NHibernateRepositoryInstaller>(),
                FromAssembly.Containing<QueryInstaller>(),

                FromAssembly.Containing<ConvertersInstaller>(),
                FromAssembly.Containing<WorkflowsInstaller>()
            );
            ServiceLocator.SetServiceLocator(container);
            WebAppContext.InitForWebApplication(container);
        }
    }
}