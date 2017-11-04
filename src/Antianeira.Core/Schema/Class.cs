using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    /// <summary>
    /// Type script Class definition
    /// </summary>
    public class Class: TsType
    {
        public Class(string name): base(name)
        { }

        [CanBeNull]
        public virtual TypeReference BaseClass { get; set; }

        public virtual bool IsAbstract { get; set; }

        [NotNull]
        public virtual ICollection<GenericParameter> Generics { get; } = new List<GenericParameter>();

        [NotNull]
        public virtual ICollection<TypeReference> Interfaces { get; } = new List<TypeReference>();

        [NotNull]
        public virtual ICollection<ClassProperty> Properties { get; } = new List<ClassProperty>();

        [NotNull]
        public virtual ICollection<ClassProperty> ConstructorProperties { get; } = new List<ClassProperty>();

        [NotNull]
        public virtual ICollection<ClassMethod> Methods { get; } = new List<ClassMethod>();

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

            writer.Append("\n");

            if (ConstructorProperties.Count > 0)
            {
                writer.Append("constructor (\n");

                writer.Join("\n", ConstructorProperties);

                writer.Append(") { }\n");
            }

            writer.Join("\n", Methods);

            writer.Append("}\n");
        }
    }
}
