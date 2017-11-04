using System;
using JetBrains.Annotations;

namespace Antianeira.Schema.MethodUrl
{
    public interface IMethodUrlQueryItem : IWritable
    {
        [CanBeNull]
        string Name { get; set; }
    }

    public abstract class QueryItem : IMethodUrlQueryItem
    {
        [CanBeNull]
        public virtual string Name { get; set; }

        public abstract void WriteValue([NotNull] IWriter writer);

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

        public override void WriteValue(IWriter writer)
        {
            writer.Append(Uri.EscapeDataString(Value));
        }
    }

    public abstract class VariableQueryItem: QueryItem
    {
        protected virtual void Wrap(IWriter writer, Action body)
        {
            writer.Append("${urlEncodeComponent(");
            body();
            writer.Append(")}");
        }
    }

    public class ParameterQueryItem : VariableQueryItem
    {
        private string _name;

        public ParameterQueryItem([NotNull] MethodParameter parameter)
        {
            Parameter = parameter;
        }

        public override string Name {
            get => _name ?? Parameter.Name;
            set => _name = value;
        }

        [NotNull]
        public MethodParameter Parameter { get; set; }

        public override void WriteValue(IWriter writer)
        {
            Wrap(writer, () =>
            {
                writer.Append(Parameter.Name);
            });
        }
    }

    public class ValueQueryItem : VariableQueryItem
    {
        private string _name;

        public ValueQueryItem([NotNull] MethodParameter parameter, [NotNull] Property property)
        {
            Parameter = parameter;
            Property = property;
        }

        public override string Name
        {
            get => _name ?? Parameter.Name + "." + Property.Name;
            set => _name = value;
        }

        [NotNull]
        public MethodParameter Parameter { get; set; }

        [NotNull]
        public Property Property { get; set; }

        public override void WriteValue(IWriter writer)
        {
            Wrap(writer, () =>
            {
                writer.Append(Parameter.Name);
                writer.Append(".");
                writer.Append(Property.Name);
            });
        }
    }
}
