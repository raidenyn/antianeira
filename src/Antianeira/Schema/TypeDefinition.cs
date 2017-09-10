using System.Collections.Generic;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace Antianeira.Schema
{
    public class TypeDefinition : TsType
    {
        public TypeDefinition(string name): base(name)
        { }

        [NotNull]
        public ICollection<string> Types { get; set; } = new List<string>();

        public override void Write(IWriter writer)
        {
            base.Write(writer);

            writer.Append("type ");
            writer.Append(Name);
            writer.Append(" = ");

            writer.Append(String.Join(" | ", Types.Select(t => "'" + t + "'")));

            writer.Append(";\n");
        }
    }
}
