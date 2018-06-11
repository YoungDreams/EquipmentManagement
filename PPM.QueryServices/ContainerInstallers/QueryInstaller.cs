using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PPM.Query.Implemention;

namespace PPM.Query.ContainerInstallers
{
    public class QueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().IncludeNonPublicTypes().InSameNamespaceAs<UserQueryService>()
                    .WithServiceAllInterfaces().LifestylePerWebRequest()
            );
        }
    }
}