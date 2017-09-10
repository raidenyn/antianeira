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
        TypeReference GetTypeReference([NotNull] Type type, [NotNull] TypeReferenceContext context);
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

        public TypeReference GetTypeReference([NotNull] Type type, [NotNull] TypeReferenceContext context)
        {
            var result = ConvertPropertyType(type, context);
            result.IsOptional = result.IsNullable = IsOptional(type, context);

            return result;
        }

        private TypeReference ConvertPropertyType(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                propertyType = nullableType;
            }

            return    AsGeneric(propertyType, context)
                   ?? AsValue(propertyType, context)
                   ?? AsDictionary(propertyType, context)
                   ?? AsArray(propertyType, context)
                   ?? AsCustom(propertyType, context)
                   ?? new AnyType();
        }

        [CanBeNull]
        private TypeReference AsValue(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            if (Types.TryGetValue(propertyType, out Type type))
            {
                return (TypeReference)Activator.CreateInstance(type);
            }

            return null;
        }

        [CanBeNull]
        private TypeReference AsGeneric(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            if (propertyType.IsGenericParameter && context.GenericParameters != null)
            {
                var genericParam = context.GenericParameters.FirstOrDefault(gp => gp.Name == propertyType.Name);

                return new GenericType
                {
                    GenericParameter = genericParam
                };
            }

            return null;
        }

        [CanBeNull]
        private TypeReference AsArray(Type propertyType, [NotNull] TypeReferenceContext context) {
            var enumerable = (from @interface in propertyType.GetInterfaces()
                              where @interface.GetTypeInfo().IsGenericType
                                    && typeof(IEnumerable<>) == @interface.GetGenericTypeDefinition()
                              select @interface).FirstOrDefault();
            if (enumerable == null)
            {
                return null;
            }

            return new ArrayType
            {
                Type = GetTypeReference(enumerable.GenericTypeArguments[0], context)
            };
        }

        [CanBeNull]
        private TypeReference AsDictionary(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            var dictionary = (from @interface in propertyType.GetInterfaces()
                              where @interface.GetTypeInfo().IsGenericType
                                    && typeof(IDictionary<,>) == @interface.GetGenericTypeDefinition()
                              select @interface).FirstOrDefault();
            if (dictionary == null)
            {
                return null;
            }

            var key = GetTypeReference(dictionary.GenericTypeArguments[0], context);
            var value = GetTypeReference(dictionary.GenericTypeArguments[1], context);

            if (!(key is NumberType) && !(key is StringType))
            {
                return null;
            }

            return new DictionaryType
            {
                Key = key,
                Value = value
            };
        }

        [CanBeNull]
        private TypeReference AsCustom(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            var tsType = _mappingSettings.DefinitionsMapper.ConvertType(propertyType.GetTypeInfo(), context.Definitions);

            if (tsType != null)
            {
                var custom = new CustomType
                {
                    Type = tsType
                };

                foreach (var generic in propertyType.GetGenericArguments())
                {
                    custom.GenericArguments.Add(GetTypeReference(generic, context));
                }

                return custom;
            }

            return null;
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
