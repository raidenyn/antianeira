using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Parameter: IWritable
    {
        public Parameter(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public ParameterType Type { get; set; } = new ParameterType();

        public virtual void Write(IWriter writer)
        {
            writer.Append(Name);

            if (Type.IsOptional)
            {
                writer.Append("?");
            }

            writer.Append(": ");
            Type.Write(writer);
        }
    }
}
