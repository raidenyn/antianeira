using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class TypeDefinition : TsType
    {
        public TypeDefinition(string name): base(name)
        { }

        [NotNull]
        public ICollection<string> Types { get; set; } = new List<string>();
    }
}
