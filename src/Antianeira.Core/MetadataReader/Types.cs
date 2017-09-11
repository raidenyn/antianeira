using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antianeira.Schema;
using Antianeira.Utils;
using JetBrains.Annotations;
using System;

namespace Antianeira.MetadataReader
{
    internal class Types
    {
        public Types(ICollection<TypeInfo> types) {
            Classes = (from type in types
                       where type.IsClass
                       select type).ToArray();
            Interfaces = (from type in types
                          where type.IsInterface
                          select type).ToArray();
            Enums = (from type in types
                     where type.IsEnum
                     select type).ToArray();
        }

        [NotNull]
        public ICollection<TypeInfo> Classes { get; }

        [NotNull]
        public ICollection<TypeInfo> Interfaces { get;}

        [NotNull]
        public ICollection<TypeInfo> Enums { get; }
    }
}
