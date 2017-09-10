using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class EnumField: Drop, IWritable
    {
        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Value { get; set; }

        public bool IsNumeric { get; set; } = true;

        [CanBeNull]
        public Comment Comment { get; set; }

        public void Write(IWriter writer)
        {
            Comment?.Write(writer);

            writer.Append(Name);
            writer.Append(" = ");

            if (IsNumeric)
            {
                writer.Append(Value);
            }
            else
            {
                writer.Append("\"").Append(Value).Append("\"");
            }
        }
    }
}
