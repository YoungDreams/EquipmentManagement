using System;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Type;
using PPM.Entities.Mappings;

namespace PPM.Entities.ContainerInstallers
{
    public class NHibernateConfigurationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mappings.hbm.xml");
            //var model = AutoMap.Assembly(typeof(User).Assembly);

            var autoMappingCfg = new NHibernateAutoMappingConfiguration();
            
            var cfg = Fluently.Configure()
                .Database(MsSqlConfiguration
                    .MsSql2012
                    .ConnectionString(c => c.FromConnectionStringWithKey("PPM"))
                    .Dialect<MsSql2008Dialect>()
                )
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<User>(autoMappingCfg)

                        .UseOverridesFromAssemblyOf<UserMapping>()
                        .OverrideAll(x => x.IgnoreProperty("RowNumber"))
                        .Conventions
                        .Setup(x =>
                        {
                            x.Add<DefaultReferenceConvention>();
                            x.Add<DefualtHasOneConvertion>();
                        })))
                //.ExposeConfiguration(c => new SchemaUpdate(c).Execute(true,true))
                .BuildConfiguration();

            cfg.SetInterceptor(new UpdateLastUpdateTimePropertyInterceptor());
            container.Register(Component.For<Configuration>().Instance(cfg).LifestyleSingleton());
        }
    }

    public class UpdateLastUpdateTimePropertyInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var modified = false;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                if ("CreatedOn" == propertyNames[i])
                {
                    state[i] = DateTime.Now;
                    modified = true;
                }

                if ("LastModifiedOn" == propertyNames[i])
                {
                    state[i] = DateTime.Now;
                    modified = true;
                }
            }
            return modified;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames,
            IType[] types)
        {
            /*
            for (int i = 0; i < propertyNames.Length; i++)
            {
                if ("LastModifiedOn" == propertyNames[i])
                {
                    currentState[i] = DateTime.Now;
                    return true;
                }
            }*/

            return false;
        }
    }

    public class NHibernateAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override string SimpleTypeCollectionValueColumn(Member member)
        {
            var old = base.SimpleTypeCollectionValueColumn(member);
            return old;
        }

        public override bool ShouldMap(Type type)
        {
            return base.ShouldMap(type) && type.IsSubclassOf(typeof(MyEntity));
        }
    }

    public class DefaultReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Name + "Id");
        }
    }

    public class DefualtHasOneConvertion : IHasOneConvention
    {
        public void Apply(IOneToOneInstance instance)
        {
            instance.ForeignKey(instance.EntityType.Name + "Id");
            instance.Cascade.SaveUpdate();
        }
    }
}
