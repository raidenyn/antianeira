using System;
using JetBrains.Annotations;
using Antianeira.Schema;
using System.Reflection;

namespace Antianeira.MetadataReader.TypeConverters
{
    public class ObjectTypeConverter : ITypeConverter
    {
        private readonly ITypeReferenceMapper _typeReferenceMapper;
        private readonly IDefinitionsMapper _definitionsMapper;

        public ObjectTypeConverter(
            ITypeReferenceMapper typeReferenceMapper,
            IDefinitionsMapper definitionsMapper
        ) {
            _typeReferenceMapper = typeReferenceMapper;
            _definitionsMapper = definitionsMapper;
        }

        public TypeReference TryConvert(Type propertyType, TypeReferenceContext context)
        {
            var tsType = _definitionsMapper.ConvertType(propertyType.GetTypeInfo(), context.GetTypeContext());

            if (tsType != null)
            {
                var custom = new CustomType(tsType)
                {
                    Source = propertyType
                };

                foreach (var generic in propertyType.GetGenericArguments())
                {
                    var type = _typeReferenceMapper.GetTypeReference(generic, context);
                    if (type != null)
                    {
                        custom.GenericArguments.Add(type);
                    }
                }

                return custom;
            }

            return null;
        }
    }
}
