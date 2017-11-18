using DotLiquid;
using JetBrains.Annotations;
using System.Collections.Generic;
using Antianeira.Schema.Renders;

namespace Antianeira.Schema
{
    public class ClassMethod : Drop, IWritable
    {
        public ClassMethod(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        public PropertyAccessLevel AccessLevel { get; set; }

        public bool IsStatic { get; set; }

        public bool IsAbstract => Body == null;

        [NotNull]
        public ReturnType Return { get; set; } = new ReturnType();

        [CanBeNull]
        public virtual IClassMethodBodyRender Body { get; set; }

        [NotNull]
        public IList<MethodParameter> Parameters { get; } = new List<MethodParameter>();

        public void Write(IWriter writer)
        {
            if (Comment != null)
            {
                Comment.Write(writer);
                writer.Append("\n");
            }

            writer.Append(AccessLevel.ToString().ToLower());
            writer.Append(" ");

            if (IsStatic)
            {
                writer.Append("static ");
            }
            else
            {
                if (IsAbstract)
                {
                    writer.Append("abstract ");
                }
            }

            writer.Append(Name);
            writer.Append("(");

            if (Parameters.Count > 0)
            {
                writer.Join(", ", Parameters);
            }

            writer.Append("): ");
            Return.Write(writer);

            if (IsAbstract || Body == null)
            {
                writer.Append(";");
            }
            else
            {
                writer.Append("{\n");
                Body.WriteBody(writer, this);
                writer.Append("\n}");
            }
        }
    }
}
