using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IPropertyTypeMapper
    {
        PropertyType GetPropertyType([NotNull] Type propertyType, [NotNull] Definitions definitions);
    }

    internal class PropertyTypeMapper : IPropertyTypeMapper
    {
        private static readonly IDictionary<Type, Type> Types = new Dictionary<Type, Type>
        {
            [typeof(string)] = typeof(StringType),
            [typeof(bool)] = typeof(BooleanType),
            [typeof(long)] = typeof(NumberType),
            [typeof(int)] = typeof(NumberType),
            [typeof(short)] = typeof(NumberType),
            [typeof(byte)] = typeof(NumberType),
            [typeof(decimal)] = typeof(NumberType),
            [typeof(float)] = typeof(NumberType),
            [typeof(double)] = typeof(NumberType),
            [typeof(object)] = typeof(ObjectType)
        };

        private readonly MappingSettings _mappingSettings;

        public PropertyTypeMapper(
            MappingSettings mappingSettings
        )
        {
            _mappingSettings = mappingSettings;
        }

        public PropertyType GetPropertyType(Type propertyType, Definitions definitions)
        {
            var result = ConvertPropertyType(propertyType, definitions);
            result.IsOptional = result.IsNullable = IsOptional(propertyType);

            return result;
        }

        private PropertyType ConvertPropertyType(Type propertyType, Definitions definitions)
        {
            if (Types.TryGetValue(propertyType, out Type type))
            {
                return (PropertyType)Activator.CreateInstance(type);
            }

            var dictionary = (from @interface in propertyType.GetInterfaces()
                              where @interface.GetTypeInfo().IsGenericType
                                    && typeof(IDictionary<,>) == @interface.GetGenericTypeDefinition()
                              select @interface).FirstOrDefault();
            if (dictionary != null)
            {
                var key = GetPropertyType(dictionary.GenericTypeArguments[0], definitions);
                var value = GetPropertyType(dictionary.GenericTypeArguments[1], definitions);

                if (key is NumberType || key is StringType)
                {
                    return new DictionaryType
                    {
                        Key = key,
                        Value = value
                    };
                }
            }

            var enumerable = (from @interface in propertyType.GetInterfaces()
                              where @interface.GetTypeInfo().IsGenericType
                                    && typeof(IEnumerable<>) == @interface.GetGenericTypeDefinition()
                              select @interface).FirstOrDefault();
            if (enumerable != null)
            {
                return new ArrayType
                {
                    Type = GetPropertyType(enumerable.GenericTypeArguments[0], definitions)
                };
            }

            var tsType = _mappingSettings.DefinitionsMapper.ConvertType(propertyType.GetTypeInfo(), definitions);

            if (tsType != null)
            {
                return new CustomType
                {
                    Type = tsType
                };
            }

            return new AnyType();
        }

        protected virtual bool IsOptional(Type propertyType) {
            return propertyType.GetTypeInfo().GetCustomAttribute<CanBeNullAttribute>() != null;
        }
    }
}
