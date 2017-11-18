using Antianeira.Schema;
using Antianeira.Utils;
using JetBrains.Annotations;
using System;
using System.Linq;
using Antianeira.MetadataReader.Comments;

namespace Antianeira.MetadataReader
{
    public interface IInterfaceMapper
    {
        [CanBeNull]
        Interface GetInterface([NotNull] Type type, [NotNull] TypeContext context);
    }

    public class DefaultInterfaceMapper : IInterfaceMapper
    {
        private readonly IInterfacePropertyMapper _interfacePropertyMapper;
        private readonly IExpendTypesProvider _expendTypesProvider;
        private readonly IExportStrategy _exportStrategy;
        private readonly ITypeReferenceMapper _typeReferenceMapper;
        private readonly IInterfacePropertiesProvider _interfacePropertiesProvider;
        private readonly ICommentsProvider _commentsProvider;
        private readonly IInterfaceNameMapper _interfaceNameMapper;

        public DefaultInterfaceMapper(
            ICommentsProvider commentsProvider,
            IInterfaceNameMapper interfaceNameMapper,
            IInterfacePropertyMapper interfacePropertyMapper,
            IExpendTypesProvider expendTypesProvider,
            IExportStrategy exportStrategy,
            ITypeReferenceMapper typeReferenceMapper,
            IInterfacePropertiesProvider interfacePropertiesProvider
        )
        {
            _interfacePropertyMapper = interfacePropertyMapper;
            _expendTypesProvider = expendTypesProvider;
            _exportStrategy = exportStrategy;
            _typeReferenceMapper = typeReferenceMapper;
            _interfacePropertiesProvider = interfacePropertiesProvider;
            _commentsProvider = commentsProvider;
            _interfaceNameMapper = interfaceNameMapper;
        }

        public Interface GetInterface(Type type, TypeContext context)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }

            var name = _interfaceNameMapper.GetInterfaceName(type);

            return context.Definitions.Interfaces.GetOrCreate(name, () => Create(name, type), @interface => Map(@interface, type, context));
        }

        private Interface Create([NotNull] string name, [NotNull] Type type)
        {
            return new Interface(name)
            {
                IsExported = _exportStrategy.IsExported(type),
                Comment = _commentsProvider.GetComment(type),
            };
        }

        private void Map([NotNull] Interface @interface, [NotNull] Type type, [NotNull] TypeContext context) {
            var typeReferenceContext = context.GetTypeReferenceContext();
            @interface.Interfaces.AddRange(from expandType in _expendTypesProvider.GetExpendTypes(type)
                                           let nestedInterface = _typeReferenceMapper.GetTypeReference(expandType, typeReferenceContext)
                                           where nestedInterface != null
                                           select nestedInterface);

            foreach (var generic in type.GetGenericArguments())
            {
                @interface.Generics.Add(new GenericParameter(generic.Name));
            }

            var interfaceContext = context.GetInterfacePropertyMappingContext(@interface.Generics);

            @interface.Properties.AddRange(from propertyInfo in _interfacePropertiesProvider.GetProperties(type)
                                           let property = _interfacePropertyMapper.MapProperty(propertyInfo, interfaceContext)
                                           where property != null
                                           select property);
        }
    }
}
