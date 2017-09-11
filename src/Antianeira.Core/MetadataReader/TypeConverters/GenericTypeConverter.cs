using System;
using JetBrains.Annotations;
using System.Linq;
using Antianeira.Schema;

namespace Antianeira.MetadataReader.TypeConverters
{
    public class GenericTypeConverter : ITypeConverter
    {
        public TypeReference TryConvert([NotNull] Type propertyType, [NotNull] TypeReferenceContext context)
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
