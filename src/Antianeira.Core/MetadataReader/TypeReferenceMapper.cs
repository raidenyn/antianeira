using System;
using System.Collections.Generic;
using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;
using Antianeira.MetadataReader.TypeConverters;

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
        public readonly IList<ITypeConverter> _converters;
        private readonly IOptionalStrategy _optionalStrategy;

        public TypeReferenceMapper(
            IList<ITypeConverter> converters,
            IOptionalStrategy optionalStrategy
        ) {
            _converters = converters;
            _optionalStrategy = optionalStrategy;
        }

        public TypeReference GetTypeReference([NotNull] Type type, [NotNull] TypeReferenceContext context)
        {
            var result = ConvertPropertyType(type, context);
            result.IsOptional = result.IsNullable = _optionalStrategy.IsOptional(type, context);

            return result;
        }

        private TypeReference ConvertPropertyType(Type propertyType, [NotNull] TypeReferenceContext context)
        {
            foreach (var converter in _converters) {
                var typeReference = converter.TryConvert(propertyType, context);
                if (typeReference != null) {
                    return typeReference;
                }
            }

            return new AnyType();
        }
    }
}
