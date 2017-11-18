using System;
using System.Linq;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public interface IMethodUrlPathSegment : IWritable
    {
        [CanBeNull]
        string SourceName { get; }
    }

    public class ConstSegment : IMethodUrlPathSegment
    {
        public string SourceName { get; } = null;

        public string Value { get; set; }

        public void Write(IWriter writer)
        {
            writer.Append(Value);
        }
    }

    public abstract class VariableSegment: IMethodUrlPathSegment
    {
        public abstract string SourceName { get; }

        [NotNull]
        public abstract TypeReference Type { get; }

        public void Write(IWriter writer)
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

    public class ParameterSegment : VariableSegment
    {
        public ParameterSegment(ServiceMethodParameter parameter)
        {
            Parameter = parameter;
        }

        [NotNull]
        public ServiceMethodParameter Parameter { get; set; }

        public override string SourceName => Parameter.Name;

        public override TypeReference Type => Parameter.Type.Types.Count == 1 ? Parameter.Type.Types.First() : new AnyType();
    }

    public class PropertySegment : VariableSegment
    {
        public ServiceMethodParameter Parameter { get; set; }

        public Property Property { get; set; }

        public override string SourceName => Parameter.Name + "." + Property.Name;

        public override TypeReference Type => Property.Type.Types.Count == 1 ? Property.Type.Types.First() : new AnyType();
    }
}
