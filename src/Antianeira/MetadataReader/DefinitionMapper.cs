using System.Linq;
using System.Reflection;
using Antianeira.Schema;
using Antianeira.Utils;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

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

        [NotNull]
        public TsType ConvertType([NotNull] TypeInfo type, [NotNull] Definitions definitions)
        {
            if (!AllowedType(type)) {
                return null;
            }

            if (type.IsInterface || type.IsClass) {
                var name = _mappingSettings.InterfaceNameMapper.GetInterfaceName(type);
                return definitions.Interfaces.GetOrCreate(name, () => GetInterface(definitions, name, type));
            }

            if (type.IsEnum)
            {
                return definitions.Types.GetOrCreate(type.Name, () => GetTypeDefinition(type));
            }

            throw new NotImplementedException($"Type '{type}' is not supported directly.");
        }

        [NotNull]
        private Interface GetInterface([NotNull] Definitions definitions, [NotNull] string name, [NotNull] TypeInfo type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition().GetTypeInfo();
            }

            var @interface = new Interface(name);

            var basedTypes = new[] { type.BaseType }.Concat(type.ImplementedInterfaces).Where(AllowedType);

            var interfaces = definitions.Interfaces.AppendList(basedTypes, _mappingSettings.InterfaceNameMapper.GetInterfaceName, (n, t) => GetInterface(definitions, n, t.GetTypeInfo()));

            foreach (var generic in type.GetGenericArguments())
            {
                @interface.Generics.Add(new GenericParameter(generic.Name));
            }

            @interface.IsExported = true;
            @interface.Interfaces.AddRange(interfaces);
            @interface.Comment = _mappingSettings.CommentsProvider.GetComment(type);
            @interface.Properties.AddRange(from propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                           let property = GetProperty(definitions, @interface.Generics, propertyInfo)
                                           where property != null
                                           select property);

            return @interface;
        }

        [CanBeNull]
        private InterfaceProperty GetProperty([NotNull] Definitions definitions, [NotNull] IList<GenericParameter> generics, [NotNull] PropertyInfo property)
        {
            var declaringType = property.DeclaringType.GetTypeInfo();

            if (declaringType.GetInterfaces().SelectMany(@interface => @interface.GetProperties()).Select(p => p.Name).Contains(property.Name))
            {
                return null;
            }

            var name = _mappingSettings.PropertyNameMapper.GetPropertyName(property);

            return new InterfaceProperty(name)
            {
                Type = _mappingSettings.PropertyTypeMapper.GetPropertyType(property.PropertyType, new PropertyTypeContext(definitions) { PropertyInfo = property, GenericParameters = generics }),
                Comment = _mappingSettings.CommentsProvider.GetComment(declaringType, property)
            };
        }

        [NotNull]
        private TypeDefinition GetTypeDefinition([NotNull] TypeInfo type)
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
