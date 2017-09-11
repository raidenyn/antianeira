using DotLiquid;
using JetBrains.Annotations;
using System.Net.Http;

namespace Antianeira.Schema
{
    public class Method: Drop
    {
        [NotNull]
        public Comment Comment { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public bool HasParams { get { return Url?.Parameters?.Structure != null; } }

        [NotNull]
        public bool HasBody { get { return Request?.Type != null; } }

        [NotNull]
        public bool HasArguments => HasParams || HasBody;

        [NotNull]
        public HttpMethod HttpMethod { get; set; }

        [NotNull]
        public MethodUrl Url { get; set; }

        [CanBeNull]
        public MethodRequest Request { get; set; }

        [CanBeNull]
        public MethodResponse Response { get; set; }
    }
}
