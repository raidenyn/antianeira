using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Interface : TsType, IWritable
    {
        public Interface(string name): base(name)
        { }

        [NotNull]
        public IList<GenericParameter> Generics { get; } = new List<GenericParameter>();

        [NotNull]
        public ICollection<TypeReference> Interfaces { get; } = new List<TypeReference>();

        [NotNull]
        public ICollection<InterfaceProperty> Properties { get; } = new List<InterfaceProperty>();

        [NotNull]
        public ICollection<InterfaceMethod> Methods { get; } = new List<InterfaceMethod>();

        public override void Write(IWriter writer)
        {
            base.Write(writer);

            writer.Append("interface ");
            writer.Append(Name);

            if (Generics.Count > 0)
            {
                writer.Append("<");

                writer.Join(", ", Generics);

                writer.Append(">");
            }

            if (Interfaces.Count > 0) {
                writer.Append(" extends ");

                writer.Join(", ", Interfaces);
            }

            writer.Append(" {\n");

            writer.Join("\n", Properties);

            writer.Join("\n", Methods);

            writer.Append("}\n");
        }
    }
}
