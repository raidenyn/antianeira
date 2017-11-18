using System;
using Antianeira.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Antianeira.MetadataReader.TypeConverters
{
    public class DictionaryTypeConverter : ITypeConverter
    {
        private readonly ITypeReferenceMapper _typeReferenceMapper;

        public DictionaryTypeConverter(
            ITypeReferenceMapper typeReferenceMapper
        ) {
            _typeReferenceMapper = typeReferenceMapper;
        }

        public TypeReference TryConvert(Type propertyType, TypeReferenceContext context)
        {
            var dictionary = (from @interface in propertyType.GetInterfaces()
                              where @interface.GetTypeInfo().IsGenericType
                                    && typeof(IDictionary<,>) == @interface.GetGenericTypeDefinition()
                              select @interface).FirstOrDefault();
            if (dictionary == null)
            {
                return null;
            }

            var key = _typeReferenceMapper.GetTypeReference(dictionary.GenericTypeArguments[0], context);
            var value = _typeReferenceMapper.GetTypeReference(dictionary.GenericTypeArguments[1], context);

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
    }
}
