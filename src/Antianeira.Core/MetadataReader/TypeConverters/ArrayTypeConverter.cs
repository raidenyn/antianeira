using System;
using JetBrains.Annotations;
using Antianeira.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Antianeira.MetadataReader.TypeConverters
{
    public class ArrayTypeConverter : ITypeConverter
    {
        private readonly ITypeReferenceMapper _typeReferenceMapper;

        public ArrayTypeConverter(
            ITypeReferenceMapper typeReferenceMapper
        ) {
            _typeReferenceMapper = typeReferenceMapper;
        }

        public TypeReference TryConvert([NotNull] Type propertyType, [NotNull] TypeReferenceContext context)
        {
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
                Type = _typeReferenceMapper.GetTypeReference(enumerable.GenericTypeArguments[0], context)
            };
        }
    }
}
