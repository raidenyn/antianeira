using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Enum : TsType, IWritable
    {
        public Enum(string name) : base(name)
        { }

        [NotNull]
        public ICollection<EnumField> Fields { get; set; } = new List<EnumField>();

        public override void Write(IWriter writer)
        {
            base.Write(writer);

            writer.Append(Name);
            writer.Append(" {\n");

            writer.Join(",\n", Fields);

            writer.Append("}");
        }
    }
}
