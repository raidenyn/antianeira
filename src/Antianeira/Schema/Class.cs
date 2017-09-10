using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    /// <summary>
    /// Type script Class definition
    /// </summary>
    public class Class: TsType, IWritable
    {
        public Class(string name): base(name)
        { }

        [CanBeNull]
        public Class BaseClass { get; set; }

        public bool IsAbstract { get; set; }

        [NotNull]
        public ICollection<GenericParameter> Generics { get; } = new List<GenericParameter>();

        [NotNull]
        public ICollection<Interface> Interfaces { get; } = new List<Interface>();

        [NotNull]
        public ICollection<ClassProperty> Properties { get; } = new List<ClassProperty>();

        public override void Write(IWriter writer)
        {
            base.Write(writer);

            writer.Append("class ");
            writer.Append(Name);

            if (Generics.Count > 0)
            {
                writer.Append("<");

                writer.Join(", ", Generics);

                writer.Append(">");
            }

            if (Interfaces.Count > 0)
            {
                writer.Append(" extends ");

                writer.Join(", ", Interfaces);
            }

            if (BaseClass != null)
            {
                writer.Append(" implements ");

                BaseClass.Write(writer);
            }

            writer.Append(" {\n");

            writer.Join("\n", Properties);

            writer.Append("}\n");
        }
    }
}
