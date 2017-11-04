using Antianeira.Schema;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace Antianeira.MetadataReader
{
    public interface ITypeDefinitionMapper
    {
        [CanBeNull]
        TypeDefinition GetTypeDefinition([NotNull] Type type, [NotNull] TypeContext context);
    }

    public class TypeDefinitionMapper : ITypeDefinitionMapper
    {
        private readonly ICommentsProvider _commentsProvider;
        private readonly ILiteralTypeMapper _literalTypeMapper;
        private readonly IEnumFieldsProvider _enumFieldsProvider;
        private readonly IExportStrategy _exportStrategy;
        private readonly ITypeDefinitionNameMapper _typeDefinitionNameMapper;

        public TypeDefinitionMapper(
            ICommentsProvider commentsProvider,
            ITypeDefinitionNameMapper typeDefinitionNameMapper,
            ILiteralTypeMapper literalTypeMapper,
            IEnumFieldsProvider enumFieldsProvider,
            IExportStrategy exportStrategy
        )
        {
            _commentsProvider = commentsProvider;
            _literalTypeMapper = literalTypeMapper;
            _enumFieldsProvider = enumFieldsProvider;
            _exportStrategy = exportStrategy;
            _typeDefinitionNameMapper = typeDefinitionNameMapper;
        }

        public TypeDefinition GetTypeDefinition([NotNull] Type type, [NotNull] TypeContext context)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }

            var name = _typeDefinitionNameMapper.GetTypeName(type);

            return context.Definitions.Types.GetOrCreate(name, () => Map(name, type, context));
        }

        protected virtual TypeDefinition Map([NotNull] string name, [NotNull] Type type, [NotNull] TypeContext context) {
            return new TypeDefinition(name)
            {
                Comment = _commentsProvider.GetComment(type),
                IsExported = _exportStrategy.IsExported(type),
                Types = (from field in _enumFieldsProvider.GetFields(type)
                         select _literalTypeMapper.GetLiteralType(field)).ToList()
            };
        }
    }
}
