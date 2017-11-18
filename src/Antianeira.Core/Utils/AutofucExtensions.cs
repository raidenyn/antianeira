using Autofac;

namespace Antianeira.Utils
{
    public static class AutofucExtensions
    {
        public static void AddSingleton<TService, TImplementation>(this ContainerBuilder services)
            where TImplementation : TService
        {
            services.RegisterType<TImplementation>().As<TService>()
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
