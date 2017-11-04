using System.Reflection;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using System.Linq;

namespace Antianeira.MetadataReader
{
    public interface IEnumFieldsProvider
    {
        [NotNull, ItemNotNull]
        IEnumerable<FieldInfo> GetFields([NotNull] Type type);
    }

    public class EnumFieldsProvider : IEnumFieldsProvider
    {
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields().Where(field => !field.IsSpecialName);
        }
    }
}
