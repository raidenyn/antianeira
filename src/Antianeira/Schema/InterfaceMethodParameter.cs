using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class InterfaceMethodParameter: Drop
    {
        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public PropertyType Type { get; set; } = new AnyType();
    }
}
