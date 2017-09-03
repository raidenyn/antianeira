using System.Linq;
using System.Reflection;
using Antianeira.Schema;
using Antianeira.Utils;
using JetBrains.Annotations;
using System;

namespace Antianeira.MetadataReader
{
    public interface IDefinitionsMapper {
        [CanBeNull]
        TsType ConvertType([NotNull] TypeInfo type, [NotNull] Definitions definitions);
    }

    public class DefinitionsMapper: IDefinitionsMapper
    {
        private readonly MappingSettings _mappingSettings;

        public DefinitionsMapper(
            MappingSettings mappingSettings
        )
        {
            _mappingSettings = mappingSettings;
        }

        public TsType ConvertType(TypeInfo type, Definitions definitions)
        {
            if (!AllowedType(type)) {
                return null;
            }

            if (type.IsInterface || type.IsClass) {
                return definitions.Interfaces.GetOrCreate(type.Name, () => GetInterface(definitions, type));
            }

            if (type.IsEnum)
            {
                return definitions.Types.GetOrCreate(type.Name, () => GetTypeDefinition(type));
            }

            throw new NotImplementedException($"Type '{type}' is not supported directly.");
        }

        private Interface GetInterface(Definitions definitions, TypeInfo type)
        {
            var @interface = new Interface(type.Name);

            var basedTypes = new[] { type.BaseType }.Concat(type.ImplementedInterfaces).Where(AllowedType);

            var interfaces = definitions.Interfaces.AppendList(basedTypes, (t) => GetInterface(definitions, t.GetTypeInfo()));

            @interface.IsExported = true;
            @interface.Interfaces.AddRange(interfaces);
            @interface.Comment = _mappingSettings.CommentsProvider.GetComment(type);
            @interface.Properties.AddRange(from property in type.GetProperties()
                                           select new InterfaceProperty(_mappingSettings.PropertyNameMapper.GetPropertyName(property))
                                           {
                                               Type = _mappingSettings.PropertyTypeMapper.GetPropertyType(property.PropertyType, definitions),
                                               Comment = _mappingSettings.CommentsProvider.GetComment(type, property)
                                           });

            return @interface;
        }

        private TypeDefinition GetTypeDefinition(TypeInfo type)
        {
            return new TypeDefinition(type.Name)
            {
                Comment = _mappingSettings.CommentsProvider.GetComment(type),
                IsExported = true,
                Types = (from field in type.GetFields()
                         where !field.IsSpecialName
                         select _mappingSettings.EnumFieldMapper.GetFieldName(field)).ToList()
            };
        }

        private bool AllowedType([CanBeNull] Type type)
        {
            if (type == null)
            {
                return false;
            }

            return !type.IsSpecialName
                && !type.IsAutoClass
                && !Equals(type.Assembly, typeof(string).GetTypeInfo().Assembly)
                && (type.IsEnum || type.GetProperties().Length > 0);
        }
    }
}
