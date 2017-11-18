using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class MethodParameter: Drop, IWritable
    {
        public MethodParameter(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public PropertyType Type { get; set; } = new PropertyType();

        public virtual void Write(IWriter writer)
        {
            writer.Append(Name);

            if (Type.IsOptional)
            {
                writer.Append("?");
            }

            writer.Append(": ");
            Type.Write(writer);
        }
    }
}
