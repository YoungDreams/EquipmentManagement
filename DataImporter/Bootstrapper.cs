using System;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Releasers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Foundation.Messaging;
using Foundation.Windsor;


namespace DataImporter
{
    public static class Bootstrapper
    {
        public static readonly string ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

        public static void Bootstrap(IWindsorContainer container)
        {
            // ReSharper disable once CSharpWarnings::CS0618
            container.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
            container.Kernel.ComponentModelBuilder.AddContributor(new LifeStyleConvertor());
            container.Register(Component.For<ICommandService>().ImplementedBy<CommandService>().LifestyleSingleton());
            container.Register(Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton());

            container.Install(
                FromAssembly.This(),
                FromAssembly.Containing<Foundation.Data.ContainerInstallers.NHibernateRepositoryInstaller>()
            );
        }


    }
}