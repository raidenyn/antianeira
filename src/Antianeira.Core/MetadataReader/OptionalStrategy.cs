using JetBrains.Annotations;
using System;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface IOptionalStrategy
    {
        bool IsOptional([NotNull] Type type, [NotNull] TypeReferenceContext context);
    }

    public class JetBrainsAttributeOptionalStrategy : IOptionalStrategy
    {
        public bool IsOptional(Type type, TypeReferenceContext context)
        {
            if (context.PropertyInfo != null)
            {
                if (type.IsGenericParameter)
                {
                    return context.PropertyInfo.GetCustomAttribute<ItemCanBeNullAttribute>() != null;
                }
                return context.PropertyInfo.GetCustomAttribute<CanBeNullAttribute>() != null;
            }

            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
