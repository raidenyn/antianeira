using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class ClassProperty : Drop, IWritable
    {
        public ClassProperty(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public TypeReference Type { get; set; } = new AnyType();

        public PropertyAccessLevel AccessLevel { get; set; }

        public bool IsStatic { get; set; }

        public bool IsAbstract { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        public void Write(IWriter writer)
        {
            Comment?.Write(writer);

            writer.Append(AccessLevel.ToString().ToLower());
            writer.Append(" ");

            if (IsStatic) {
                writer.Append("static ");
            } else {
                if (IsAbstract)
                {
                    writer.Append("abstract ");
                }
            }

            writer.Append(Name);

            if (Type.IsOptional) {
                writer.Append("?");
            }

            writer.Append(": ");
            Type.Write(writer);
            writer.Append(";");
        }
    }

    public class InterfaceProperty : Drop, IWritable
    {
        public InterfaceProperty(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public TypeReference Type { get; set; } = new AnyType();

        [CanBeNull]
        public Comment Comment { get; set; }

        public void Write(IWriter writer)
        {
            Comment?.Write(writer);

            writer.Append(Name);

            if (Type.IsOptional)
            {
                writer.Append("?");
            }

            writer.Append(": ");
            Type.Write(writer);
            writer.Append(";");
        }
    }
}
