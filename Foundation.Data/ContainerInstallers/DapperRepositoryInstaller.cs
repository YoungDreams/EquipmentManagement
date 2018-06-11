using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Foundation.Core;
using Foundation.Data.Implemention;
using Foundation.Data.UnitOfWork;

namespace Foundation.Data.ContainerInstallers
{
    public class DapperRepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IRepository,IFetcher>()
                         .ImplementedBy<DapperRepository>()
                         .LifestylePerWebRequest(),
                Component.For<IUnitOfWork>()
                         .ImplementedBy<DefaultUnitOfWork>()
                         .LifestyleScoped()                
            );
        }
    }
}