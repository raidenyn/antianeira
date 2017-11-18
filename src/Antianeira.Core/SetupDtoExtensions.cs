using Antianeira.Formatters;
using Antianeira.Formatters.Filters;
using Antianeira.MetadataReader;
using Antianeira.MetadataReader.Comments;
using Antianeira.MetadataReader.TypeConverters;
using Antianeira.Schema;
using Antianeira.Utils;
using Autofac;

namespace Antianeira
{
    public static class SetupDtoExtensions
    {
        public static ContainerBuilder AddDtoReader(this ContainerBuilder services)
        {
            services.AddSingleton<ITypeReferenceMapper, TypeReferenceMapper>();
            services.AddSingleton<ITypeFilterStrategy, DefaultTypeFilterStrategy>();
            services.AddSingleton<ITypeDefinitionNameMapper, TypeDefinitionNameMapper>();
            services.AddSingleton<ITypeDefinitionMapper, TypeDefinitionMapper>();
            services.AddSingleton<IPropertyNameMapper, CamelCasePropertyTypeName>();
            services.AddSingleton<IPropertyTypeMapper, DefaultPropertyTypeMapper>();
            services.AddSingleton<IParameterTypeMapper, DefaultParameterTypeMapper>();
            services.AddSingleton<IReturnTypeMapper, DefaultReturnTypeMapper>();
            services.AddSingleton<IPropertyNullableStrategy, NeverNullableStrategy>();
            services.AddSingleton<IPropertyOptionalStrategy, JetBrainsAttributeOptionalStrategy>();
            services.AddSingleton<IParameterNullableStrategy, NeverNullableStrategy>();
            services.AddSingleton<IParameterOptionalStrategy, JetBrainsAttributeOptionalStrategy>();
            services.AddSingleton<IReturnNullableStrategy, NeverNullableStrategy>();
            services.AddSingleton<IReturnOptionalStrategy, JetBrainsAttributeOptionalStrategy>();
            services.AddSingleton<IReturnTypeMapper, DefaultReturnTypeMapper>();
            services.AddSingleton<ILiteralTypeMapper, CamelCaseLiteralTypeMapper>();
            services.AddSingleton<IInterfacePropertyMapper, DefaultInterfacePropertyMapper>();
            services.AddSingleton<IInterfacePropertiesProvider, OnlyOwnInterfacePropertiesProvider>();
            services.AddSingleton<IInterfaceNameMapper, DefaultInterfaceNameMapper>();
            services.AddSingleton<IInterfaceMapper, DefaultInterfaceMapper>();
            services.AddSingleton<IExportStrategy, AlwaysExportStrategy>();
            services.AddSingleton<IExpendTypesProvider, InterfaceExpendTypesProvider>();
            services.AddSingleton<IEnumFieldsProvider, EnumFieldsProvider>();
            services.AddSingleton<IEnumFieldMapper, CamelCaseEnumFieldMapper>();
            services.AddSingleton<IDefinitionsMapper, DefinitionsMapper>();
            services.AddSingleton<ICommentsProvider, AssemblyCommentsProvider>();

            services.AddSingleton<ITypeConverter, GenericTypeConverter>();
            services.AddSingleton<ITypeConverter, ValueTypeConverter>();
            services.AddSingleton<ITypeConverter, DictionaryTypeConverter>();
            services.AddSingleton<ITypeConverter, ArrayTypeConverter>();
            services.AddSingleton<ITypeConverter, ObjectTypeConverter>();

            return services;
        }

        public static ContainerBuilder AddDtoFormatter(this ContainerBuilder services)
        {
            var assembly = typeof(SetupDtoExtensions).Assembly;

            services.Register(c => FormatterTemplates.Current)
                .SingleInstance()
                .OnActivated(@event =>
                    @event.Instance.Register<Definitions>(new FormatterTemplate(assembly, "definitions", new[] {typeof(DefinitionsFilters)})));

            return services;
        }
    }

}
