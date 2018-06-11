using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Releasers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Foundation.Core;
using Foundation.Messaging;
using PensionInsurance.Calculator.ContainerInstallers;
using PensionInsurance.CommandHandlers.ContainerInstallers;
using PensionInsurance.Entities.ContainerInstallers;
using PensionInsurance.NC.ContainerInstallers;
using PensionInsurance.Payments.ContainerInstallers;
using PensionInsurance.Query.ContainerInstallers;
using PensionInsurance.Converters.ContainerInstallers;
using PensionInsurance.SendEmail.ContainerInstallers;
using PensionInsurance.Workflows.ContainerInstallers;

namespace PensionInsurance.Web
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
                FromAssembly.Containing<CalculatorsInstaller>(),
                FromAssembly.Containing<PaymentsInstaller>(),
                FromAssembly.Containing<NCInstaller>(),
                FromAssembly.Containing<ConvertersInstaller>(),
                FromAssembly.Containing<WorkflowsInstaller>(),
                FromAssembly.Containing<SendEmailInstaller>()
            );
            ServiceLocator.SetServiceLocator(container);
            Shared.WebAppContext.InitForWebApplication(container);
        }
    }
}