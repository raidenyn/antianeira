using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public static class TypeReferenceDefinitionExtensions
    {
        [NotNull, ItemNotNull]
        public static IEnumerable<IStructure> GetStructures([NotNull, ItemNotNull] this IEnumerable<TypeReference> types)
        {
            return from type in types
                let customType = type as CustomType
                let genericType = type as GenericType
                let structure = (customType?.Type as IStructure)
                where structure != null
                select structure;
        }
    }
}
