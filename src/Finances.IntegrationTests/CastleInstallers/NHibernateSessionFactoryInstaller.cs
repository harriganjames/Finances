
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Persistence.FNH.Mappings;
using FluentNHibernate.Cfg;
using NHibernate;

namespace Finances.IntegrationTests.CastleInstallers
{
    class NHibernateSessionFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            string conn = "Server=localhost; Database=FinanceINT; Integrated Security=true";// ConfigurationManager.AppSettings["localDB"];


            container.Register(Component.For<ISessionFactory>().UsingFactoryMethod(() =>
                Fluently.Configure()
                    .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(conn))
                        .ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BankMap>()
                        .ExportTo(@"C:\Temp"))
                    .BuildSessionFactory()
                ));


        }
    }
}
