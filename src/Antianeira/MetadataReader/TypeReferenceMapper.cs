using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface ITypeReferenceMapper
    {
        TypeReference GetPropertyType([NotNull] Type type, [NotNull] TypeReferenceContext context);
    }

    public class TypeReferenceContext {
        public TypeReferenceContext(Definitions definitions)
        {
            Definitions = definitions;
        }

        [NotNull]
        public Definitions Definitions { get; }

        [CanBeNull]
        public IList<GenericParameter> GenericParameters { get; set; }

        [CanBeNull]
        public PropertyInfo PropertyInfo { get; set; }
    }

    internal class TypeReferenceMapper : ITypeReferenceMapper
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
            [typeof(object)] = typeof(ObjectType),
            [typeof(DateTime)] = typeof(DateType)
        };

        private readonly MappingSettings _mappingSettings;

        public TypeReferenceMapper(
            MappingSettings mappingSettings
        )
        {
            _mappingSettings = mappingSettings;
        }

        public TypeReference GetPropertyType([NotNull] Type type, [NotNull] TypeReferenceContext context)
        {
            var result = ConvertPropertyType(type, context);
            result.IsOptional = result.IsNullable = IsOptional(type, context);

            return result;
        }

        private TypeReference ConvertPropertyType(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            if (propertyType.IsGenericParameter && context.GenericParameters != null)
            {
                var genericParam = context.GenericParameters.FirstOrDefault(gp => gp.Name == propertyType.Name);

                return new GenericType
                {
                    GenericParameter = genericParam
                };
            }

            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                propertyType = nullableType;
            }

            if (Types.TryGetValue(propertyType, out Type type))
            {
                return (TypeReference)Activator.CreateInstance(type);
            }

            var dictionary = (from @interface in propertyType.GetInterfaces()
                              where @interface.GetTypeInfo().IsGenericType
                                    && typeof(IDictionary<,>) == @interface.GetGenericTypeDefinition()
                              select @interface).FirstOrDefault();
            if (dictionary != null)
            {
                var key = GetPropertyType(dictionary.GenericTypeArguments[0], context);
                var value = GetPropertyType(dictionary.GenericTypeArguments[1], context);

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
                    Type = GetPropertyType(enumerable.GenericTypeArguments[0], context)
                };
            }

            var tsType = _mappingSettings.DefinitionsMapper.ConvertType(propertyType.GetTypeInfo(), context.Definitions);

            if (tsType != null)
            {
                var custom = new CustomType
                {
                    Type = tsType
                };

                foreach (var generic in propertyType.GetGenericArguments())
                {
                    custom.GenericArguments.Add(GetPropertyType(generic, context));
                }

                return custom;
            }

            return new AnyType();
        }

        protected virtual bool IsOptional([NotNull] Type type, [NotNull] TypeReferenceContext context) {
            if (context.PropertyInfo != null)
            {
                if (type.IsGenericParameter) {
                    return context.PropertyInfo.GetCustomAttribute<ItemCanBeNullAttribute>() != null;
                }
                return context.PropertyInfo.GetCustomAttribute<CanBeNullAttribute>() != null;
            }

            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
