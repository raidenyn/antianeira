using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class InterfaceMethodParameter: Drop, IWritable
    {
        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public TypeReference Type { get; set; } = new AnyType();

        public void Write(IWriter writer)
        {
            writer.Append(Name);
            writer.Append(": ");
            Type.Write(writer);
        }
    }
}
