using DotLiquid;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Antianeira.Schema
{
    public class InterfaceMethod: Drop
    {
        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        [NotNull]
        public PropertyType Return { get; set; } = new VoidType();

        [NotNull]
        public IList<InterfaceMethodParameter> Parameters { get; } = new List<InterfaceMethodParameter>();
    }
}
