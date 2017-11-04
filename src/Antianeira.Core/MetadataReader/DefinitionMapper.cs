using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;
using System;

namespace Antianeira.MetadataReader
{
    public interface IDefinitionsMapper {
        [CanBeNull]
        TsType ConvertType([NotNull] TypeInfo type, [NotNull] TypeContext context);
    }

    public class DefinitionsMapper: IDefinitionsMapper
    {
        public ITypeDefinitionMapper TypeDefinitionMapper { get; set; }
        public IInterfaceMapper InterfaceMapper { get; set; }
        public ITypeFilterStrategy TypeFilterStrategy { get; set; }

        [CanBeNull]
        public TsType ConvertType(TypeInfo type, TypeContext context)
        {
            if (!TypeFilterStrategy.IsAllowedType(type)) {
                return null;
            }

            if (type.IsInterface || type.IsClass) {
                return InterfaceMapper.GetInterface(type, context);
            }

            if (type.IsEnum)
            {
                return TypeDefinitionMapper.GetTypeDefinition(type, context);
            }

            throw new NotImplementedException($"Type '{type}' is not supported directly.");
        }
    }
}
