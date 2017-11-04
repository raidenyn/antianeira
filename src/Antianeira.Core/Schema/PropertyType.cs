using DotLiquid;
using System.Collections.Generic;

namespace Antianeira.Schema
{
    public class ReturnType : Drop, IWritable
    {
        public bool IsNullable { get; set; }

        public bool IsOptional { get; set; }

        public ICollection<TypeReference> Types { get; } = new List<TypeReference>();

        public void Write(IWriter writer)
        {
            if (Types.Count > 0)
            {
                writer.Join(" | ", Types);
            }
            else
            {
                writer.Append("any");
            }

            if (IsNullable)
            {
                writer.Append(" | null");
            }

            if (IsOptional)
            {
                writer.Append(" | undefined");
            }
        }
    }
}
