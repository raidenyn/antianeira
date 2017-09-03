using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class EnumField: Drop
    {
        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Value { get; set; }

        public bool IsNumeric { get; set; } = true;

        [CanBeNull]
        public Comment Comment { get; set; }
    }
}
