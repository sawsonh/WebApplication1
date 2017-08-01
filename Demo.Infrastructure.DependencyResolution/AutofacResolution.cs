using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Demo.Core.Repositories;
using Demo.Core.Services;
using Demo.Infrastructure.Repositories;
using Demo.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Demo.Infrastructure.DependencyResolution
{
    public class AutofacResolution
    {
        public static void RegisterTypes(ContainerBuilder builder, IConfigurationRoot cfgRoot)
        {
            builder.RegisterType<LoggingService>().As<ILoggingService>();
            builder.RegisterType<ConfigService>().As<IConfigService>().WithParameter("cfgRoot", cfgRoot);
            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
        }
    }
}
