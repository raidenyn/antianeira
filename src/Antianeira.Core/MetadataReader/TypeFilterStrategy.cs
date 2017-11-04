using JetBrains.Annotations;
using System;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface ITypeFilterStrategy
    {
        bool IsAllowedType([NotNull] Type type);
    }

    public class DefaultTypeFilterStrategy : ITypeFilterStrategy
    {
        public bool IsAllowedType(Type type) {
            return !type.IsSpecialName
                && !type.IsAutoClass
                && !Equals(type.Assembly, typeof(string).GetTypeInfo().Assembly)
                && (type.IsEnum || type.GetProperties().Length > 0);
        }
    }
}
