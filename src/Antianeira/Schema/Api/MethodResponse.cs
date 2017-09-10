using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema.Api
{
    public class MethodResponse : Drop
    {
        [CanBeNull]
        public TypeReference Type { get; set; }
    }
}
