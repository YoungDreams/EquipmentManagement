using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Foundation.Core;
using Foundation.Data.Implemention;
using Foundation.Data.UnitOfWork;
using NHibernate;
using NHibernate.Cfg;

namespace Foundation.Data.ContainerInstallers
{
    public class NHibernateRepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<NHibernate.ISessionFactory>()
                         .UsingFactoryMethod(k => k.Resolve<Configuration>().BuildSessionFactory())
                         .LifestyleSingleton(),

                Component.For<ISession>()
                         .UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenSession())
                         .LifestylePerWebRequest(),

                Component.For<IStatelessSession>()
                         .UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenStatelessSession())
                         .LifestylePerWebRequest(),

                Component.For<IProvider<ISession>>()
                         .ImplementedBy<WindsorProvider<ISession>>()
                         .LifestyleSingleton(),

                Component.For<IRepository, IFetcher>()
                         .ImplementedBy<NHibernateRepository>()
                         .LifestylePerWebRequest(),
                Component.For<IUnitOfWork>()
                         .ImplementedBy<NHibernateUnitOfWork>()
                         .LifestyleScoped()
            );


        }
    }
}