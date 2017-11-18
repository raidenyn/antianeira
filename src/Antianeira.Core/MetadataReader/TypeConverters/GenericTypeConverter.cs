using System;
using System.Linq;
using Antianeira.Schema;

namespace Antianeira.MetadataReader.TypeConverters
{
    public class GenericTypeConverter : ITypeConverter
    {
        public TypeReference TryConvert(Type propertyType, TypeReferenceContext context)
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
    }
}
