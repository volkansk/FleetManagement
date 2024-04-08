using Autofac;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Repository;
using FleetManagement.Repository.UnitOfWork;
using FleetManagement.Service.Services;
using System.Reflection;

namespace FleetManagement.API.Modules
{
    public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(DataContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(BagService));

#pragma warning disable CS8604 // Possible null reference argument.
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
                .Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().
                InstancePerLifetimeScope();
#pragma warning restore CS8604 // Possible null reference argument.

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
                .Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().
                InstancePerLifetimeScope();
        }
    }
}
