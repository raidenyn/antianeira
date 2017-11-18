using System;
using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IPropertyTypeMapper
    {
        [NotNull]
        PropertyType GetPropertyType([NotNull] PropertyInfo property, [NotNull] TypeReferenceContext context);
    }

    internal class DefaultPropertyTypeMapper : IPropertyTypeMapper
    {
        private readonly IPropertyOptionalStrategy _optionalStrategy;
        private readonly ITypeReferenceMapper _typeReference;
        private readonly IPropertyNullableStrategy _nullableStrategy;

        public DefaultPropertyTypeMapper(
            ITypeReferenceMapper typeReference,
            IPropertyOptionalStrategy optionalStrategy,
            IPropertyNullableStrategy nullableStrategy
        ) {
            _optionalStrategy = optionalStrategy;
            _typeReference = typeReference;
            _nullableStrategy = nullableStrategy;
        }

        public PropertyType GetPropertyType(PropertyInfo property, TypeReferenceContext context)
        {
            TypeReference typeReference = _typeReference.GetTypeReference(property.PropertyType, context) ?? new AnyType();

            return new PropertyType
            {
                IsOptional = _optionalStrategy.IsOptional(property, context),
                IsNullable = _nullableStrategy.IsNullable(property, context),
                Types = { typeReference }
            };
        }
    }
}
