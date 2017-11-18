using System;
using Antianeira.Schema;
using System.Collections.Generic;

namespace Antianeira.MetadataReader.TypeConverters
{
    public static class PredefinedValueTypes {
        public static readonly IReadOnlyDictionary<Type, Type> Types = new Dictionary<Type, Type>
        {
            [typeof(string)] = typeof(StringType),
            [typeof(bool)] = typeof(BooleanType),
            [typeof(long)] = typeof(NumberType),
            [typeof(int)] = typeof(NumberType),
            [typeof(short)] = typeof(NumberType),
            [typeof(byte)] = typeof(NumberType),
            [typeof(decimal)] = typeof(NumberType),
            [typeof(float)] = typeof(NumberType),
            [typeof(double)] = typeof(NumberType),
            [typeof(object)] = typeof(ObjectType),
            [typeof(DateTime)] = typeof(DateType)
        };
    }

    public class ValueTypeConverter : ITypeConverter
    {
        public virtual IReadOnlyDictionary<Type, Type> PredefinedTypes => PredefinedValueTypes.Types;

        public TypeReference TryConvert(Type propertyType, TypeReferenceContext context)
        {
            if (PredefinedTypes.TryGetValue(propertyType, out Type type))
            {
                return (TypeReference)Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
