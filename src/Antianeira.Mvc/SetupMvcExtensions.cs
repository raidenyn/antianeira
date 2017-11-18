using Antianeira.Formatters;
using Antianeira.Formatters.Filters;
using Antianeira.MetadataReader;
using Antianeira.Schema;
using Antianeira.Utils;
using Autofac;

namespace Antianeira
{
    public static class SetupMvcExtensions
    {
        public static ContainerBuilder AddMvcReader(this ContainerBuilder services)
        {
            services.AddDtoReader();

            services.AddSingleton<IMethodNameMapper, CamelCaseMethodName>();
            services.AddSingleton<IActionChecker, MvcCoreActionChecker>();
            services.AddSingleton<IControllerChecker, MvcControllerChecker>();
            services.AddSingleton<IHttpMethodMapper, AttributeHttpMethodMapper>();
            services.AddSingleton<IMethodReponseMapper, MethodReponseMapper>();
            services.AddSingleton<IParameterNameMapper, CamelCaseParameterNameMapper>();
            services.AddSingleton<IParameterPassOverMapper, AttributeParameterPassOverMapper>();
            services.AddSingleton<IServiceMethodMapper, DefaultServiceMethodMapper>();
            services.AddSingleton<IServiceMethodParameterMapper, ServiceMethodParameterMapper>();
            services.AddSingleton<IServiceNameMapper, DefaultServiceNameMapper>();
            services.AddSingleton<IMethodUrlMapper, MethodUrlMapper>();
            services.AddSingleton<IUrlTemplateMapper, AttributeUrlTemplateMapper>();
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
                templates.Register<ServicesDefinitions>(
                    new FormatterTemplate(assembly, "services", new[] {typeof(DefinitionsFilters), typeof(ServicesFilters)}));
                templates.Register<Service>(
                    new FormatterTemplate(assembly, "client", new[] { typeof(ServiceMethodFilters) }));
            });

            return services;
        }
    }

}
