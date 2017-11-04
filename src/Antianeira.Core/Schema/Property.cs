using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public abstract class Property : Drop, IWritable
    {
        protected Property(string name)
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
            writer.Append(";\n");
        }
    }

    public class ClassProperty : Property
    {
        public ClassProperty(string name): base(name)
        { }

        public PropertyAccessLevel AccessLevel { get; set; }

        public bool IsStatic { get; set; }

        public bool IsReadonly { get; set; }

        public bool IsAbstract { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        public override void Write(IWriter writer)
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

            if (IsReadonly) {
                writer.Append("readonly ");
            }

            base.Write(writer);
        }
    }

    public class InterfaceProperty : Property
    {
        public InterfaceProperty(string name): base(name)
        { }

        public bool IsReadonly { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        public override void Write(IWriter writer)
        {
            Comment?.Write(writer);

            if (IsReadonly)
            {
                writer.Append("readonly ");
            }

            base.Write(writer);
        }
    }
}
