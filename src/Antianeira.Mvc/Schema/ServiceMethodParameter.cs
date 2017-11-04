using System.Reflection;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class ServiceClientMethodParameter : MethodParameter
    {
        public ServiceClientMethodParameter(string name)
            : base(name)
        { }

        public PassType PassType { get; set; }

        [CanBeNull]
        public ParameterInfo SourceParameter { get; set; }
    }
}
