using System.Reflection;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class ServiceMethodParameter
    {
        public ServiceMethodParameter([NotNull] string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        public ParameterPassOver PassOver { get; set; }

        [NotNull]
        public ParameterType Type { get; set; } = new ParameterType();

        [CanBeNull]
        public ParameterInfo Source { get; set; }
    }
}
