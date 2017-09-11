using DotLiquid;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Antianeira.Schema
{
    public class InterfaceMethod: Drop, IWritable
    {
        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        [NotNull]
        public TypeReference Return { get; set; } = new VoidType();

        [NotNull]
        public IList<InterfaceMethodParameter> Parameters { get; } = new List<InterfaceMethodParameter>();

        public void Write(IWriter writer)
        {
            if (Comment != null)
            {
                Comment.Write(writer);
                writer.Append("\n");
            }

            writer.Append(Name);
            writer.Append("(");

            if (Parameters.Count > 0)
            {
                writer.Join(", ", Parameters);
            }

            writer.Append("): ");
            Return.Write(writer);
            writer.Append(";");
        }
    }
}
