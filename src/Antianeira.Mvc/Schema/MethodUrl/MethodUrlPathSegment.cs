using System;

namespace Antianeira.Schema.MethodUrl
{
    public interface IMethodUrlPathSegment : IWritable
    { }

    public class ConstSegment : IWritable
    {
        public string Value { get; set; }

        public void Write(IWriter writer)
        {
            writer.Append(Value);
        }
    }

    public abstract class VariableSegment: IMethodUrlPathSegment
    {
        protected virtual void Wrap(IWriter writer, Action body)
        {
            writer.Append("${urlEncodeComponent(");
            body();
            writer.Append(")}");
        }

        public abstract void Write(IWriter writer);
    }

    public class ParameterSegment : VariableSegment
    {
        public ServiceClientMethodParameter Parameter { get; set; }

        public override void Write(IWriter writer)
        {
            Wrap(writer, () =>
            {
                writer.Append(Parameter.Name);
            });
        }
    }

    public class ValueSegment : VariableSegment
    {
        public ServiceClientMethodParameter Parameter { get; set; }

        public Property Property { get; set; }

        public override void Write(IWriter writer)
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
