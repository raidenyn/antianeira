using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public abstract class TsType : Drop, ITsType
    {
        protected TsType(string name) {
            Name = name;
        }

        [NotNull]
        public string Name { get; internal set; }

        public bool IsExported { get; set; }

        public bool IsDeclared { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        public virtual void Write(IWriter writer)
        {
            Comment?.Write(writer);

            if (IsDeclared)
            {
                writer.Append("declare ");
            }

            if (IsExported)
            {
                writer.Append("export ");
            }
        }
    }
}
