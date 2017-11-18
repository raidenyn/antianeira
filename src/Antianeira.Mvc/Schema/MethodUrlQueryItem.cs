using System;
using System.Linq;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public interface IMethodUrlQueryItem : IWritable
    {
        [CanBeNull]
        string Name { get; }

        [CanBeNull]
        string SourceName { get; }
    }

    public abstract class QueryItem : Drop, IMethodUrlQueryItem
    {
        public virtual string Name { get; set; }

        public abstract string SourceName { get; }

        protected abstract void WriteValue([NotNull] IWriter writer);

        public void Write(IWriter writer)
        {
            writer.Append(Uri.EscapeDataString(Name));
            writer.Append("=");
            WriteValue(writer);
        }
    }

    public class ConstQueryItem : QueryItem
    {
        [CanBeNull]
        public string Value { get; set; }

        public override string SourceName { get; } = null;

        protected override void WriteValue(IWriter writer)
        {
            writer.Append(Uri.EscapeDataString(Value));
        }
    }

    public abstract class VariableQueryItem: QueryItem
    {
        [NotNull]
        public abstract TypeReference Type { get; }

        protected override void WriteValue(IWriter writer)
        {
            writer.Append("${encodeURIComponent(");
            writer.Append(SourceName);
            writer.Append(GetCastFunction());
            writer.Append(")}");
        }

        public string GetCastFunction()
        {
            if (Type is StringType)
            {
                return "";
            }
            if (Type is DateType)
            {
                return ".toISOString()";
            }
            return ".toString()";
        }
    }

    public class ParameterQueryItem : VariableQueryItem
    {
        private string _name;

        public ParameterQueryItem([NotNull] ServiceMethodParameter parameter)
        {
            Parameter = parameter;
        }

        public override string Name {
            get => _name ?? Parameter.Name;
            set => _name = value;
        }

        public override string SourceName => Parameter.Name;

        [NotNull]
        public ServiceMethodParameter Parameter { get; set; }

        public override TypeReference Type => Parameter.Type.Types.Count == 1 ? Parameter.Type.Types.First() : new AnyType();
    }

    public class ValueQueryItem : VariableQueryItem
    {
        private string _name;

        public ValueQueryItem([NotNull] ServiceMethodParameter parameter, [NotNull] Property property)
        {
            Parameter = parameter;
            Property = property;
        }

        public override string Name
        {
            get => _name ?? Property.Name;
            set => _name = value;
        }

        public override string SourceName => $"{Parameter.Name}.{Property.Name}";

        [NotNull]
        public ServiceMethodParameter Parameter { get; set; }

        [NotNull]
        public Property Property { get; set; }

        public override TypeReference Type => Property.Type.Types.Count == 1 ? Property.Type.Types.First() : new AnyType();
    }
}
