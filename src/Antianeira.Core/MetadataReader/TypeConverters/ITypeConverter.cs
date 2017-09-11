using Antianeira.Schema;
using JetBrains.Annotations;
using System;

namespace Antianeira.MetadataReader.TypeConverters
{
    interface ITypeConverter
    {
        [CanBeNull]
        TypeReference TryConvert([NotNull] Type propertyType, [NotNull] TypeReferenceContext context);
    }
}
