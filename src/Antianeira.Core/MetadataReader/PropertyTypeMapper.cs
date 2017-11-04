using System;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IPropertyTypeMapper
    {
        [NotNull]
        PropertyType GetPropertyType([NotNull] Type type, [NotNull] TypeReferenceContext context);
    }

    internal class DefaultPropertyTypeMapper : IPropertyTypeMapper
    {
        private readonly IOptionalStrategy _optionalStrategy;
        private readonly ITypeReferenceMapper _typeReference;
        private readonly INullableStrategy _nullableStrategy;

        public DefaultPropertyTypeMapper(
            ITypeReferenceMapper typeReference,
            IOptionalStrategy optionalStrategy,
            INullableStrategy nullableStrategy
        ) {
            _optionalStrategy = optionalStrategy;
            _typeReference = typeReference;
            _nullableStrategy = nullableStrategy;
        }

        public PropertyType GetPropertyType([NotNull] Type type, [NotNull] TypeReferenceContext context)
        {
            TypeReference typeReference = _typeReference.GetTypeReference(type, context) ?? new AnyType();

            return new PropertyType {
                IsOptional = _optionalStrategy.IsOptional(type, context),
                IsNullable = _nullableStrategy.IsNullable(type, context),
                Types = { typeReference }
            };
        }
    }
}
