using System.Collections.Generic;
using System.Linq;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public abstract class TypeReferenceDefinition: Drop, IWritable
    {
        public bool IsNullable { get; set; }

        public bool IsOptional { get; set; }

        [NotNull, ItemNotNull]
        public ICollection<TypeReference> Types { get; } = new List<TypeReference>();

        public bool? IsSimple
        {
            get
            {
                if (Types.All(t => t.IsSimple == true))
                {
                    return true;
                }
                if (Types.All(t => t.IsSimple == false))
                {
                    return false;
                }
                return null;
            }
        }

        public virtual void Write(IWriter writer)
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
        }
    }
}
