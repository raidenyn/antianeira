using System;
using System.Collections.Generic;
using Antianeira.Schema;
using JetBrains.Annotations;
using Antianeira.MetadataReader.TypeConverters;

namespace Antianeira.MetadataReader
{
    public interface ITypeReferenceMapper
    {
        [CanBeNull]
        TypeReference GetTypeReference([NotNull] Type type, [NotNull] TypeReferenceContext context);
    }

    internal class TypeReferenceMapper : ITypeReferenceMapper
    {
        public IList<ITypeConverter> Converters { get; set; }
        public ITypeFilterStrategy TypeFilterStrategy { get; set; }

        public TypeReference GetTypeReference(Type type, TypeReferenceContext context)
        {
            if (TypeFilterStrategy.IsAllowedType(type))
            {
                foreach (var converter in Converters)
                {
                    var typeReference = converter.TryConvert(type, context);
                    if (typeReference != null)
                    {
                        return typeReference;
                    }
                }
            }

            return null;
        }
    }
}
