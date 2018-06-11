using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentValidation;

//using PPM.CommandHandlers.Extensions;

namespace PPM.CommandHandlers.ContainerInstallers
{
    public class FluentValidationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn(typeof(AbstractValidator<>)).WithServiceFirstInterface().LifestylePerWebRequest());
        }
    }
}
