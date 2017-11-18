using System.Reflection;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class ServiceMethodResponse
    {
        [CanBeNull]
        public MethodInfo SourceMethod { get; set; }

        [NotNull]
        public ReturnType Type { get; set; } = new ReturnType();
    }
}
