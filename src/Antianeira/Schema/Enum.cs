using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Enum : TsType
    {
        public Enum(string name) : base(name)
        { }

        [NotNull]
        public ICollection<EnumField> Fields { get; set; } = new List<EnumField>();
    }
}
