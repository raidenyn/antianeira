using JetBrains.Annotations;
using System;

namespace Antianeira.MetadataReader
{
    public interface INullableStrategy
    {
        bool IsNullable([NotNull] Type type, [NotNull] TypeReferenceContext context);
    }

    public class NeverNullableStrategy : INullableStrategy
    {
        public bool IsNullable(Type type, TypeReferenceContext context)
        {
            return false;
        }
    }
}
