using Antianeira.Formatters;
using Antianeira.Formatters.Filters;
using Antianeira.MetadataReader;
using Antianeira.Schema;
using Autofac;

namespace Antianeira
{
    public static class SetupMvcExtensions
    {
        public static ContainerBuilder AddMvcReader(this ContainerBuilder services)
        {
            services.AddDtoReader();

            services.AddSingleton<IMethodNameMapper, CamelCaseMethodName>();
            services.AddSingleton<IApiControllerReader, ApiControllerReader>();


            return services;
        }

        public static ContainerBuilder AddMvcFormatter(this ContainerBuilder services)
        {
            var assembly = typeof(SetupMvcExtensions).Assembly;

            services.AddDtoFormatter();

            services.RegisterBuildCallback(registry =>
            {
                var templates = registry.Resolve<IFormatterTemplates>();
                templates.Register<Services>(
                    new FormatterTemplate(assembly, "services", new[] {typeof(DefinitionsFilters), typeof(ServicesFilters)}));
                templates.Register<Service>(
                    new FormatterTemplate(assembly, "client", new[] { typeof(ServiceClientFilters) }));
            });

            return services;
        }
    }

}
