using System;
using JetBrains.Annotations;
using Antianeira.Schema;
using System.Collections.Generic;
using System.Linq;
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

        public TypeReference TryConvert([NotNull] Type propertyType, [NotNull] TypeReferenceContext context)
        {
            var tsType = _definitionsMapper.ConvertType(propertyType.GetTypeInfo(), context.Definitions);

            if (tsType != null)
            {
                var custom = new CustomType
                {
                    Type = tsType
                };

                foreach (var generic in propertyType.GetGenericArguments())
                {
                    custom.GenericArguments.Add(_typeReferenceMapper.GetTypeReference(generic, context));
                }

                return custom;
            }

            return null;
        }
    }
}
