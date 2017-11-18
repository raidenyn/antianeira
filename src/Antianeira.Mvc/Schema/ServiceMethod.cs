using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using System.Net.Http;
using System.Reflection;
using DotLiquid;

namespace Antianeira.Schema
{
    public class ServiceMethod: Drop
    {
        public ServiceMethod([NotNull] string name)
        {
            Name = name;
        }

        [CanBeNull]
        public MethodInfo SourceMethod { get; set; }

        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        [NotNull]
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;

        [NotNull]
        public MethodUrl Url { get; set; } = new MethodUrl();

        [NotNull]
        public List<ServiceMethodParameter> Parameters { get; set; } = new List<ServiceMethodParameter>(0);

        [NotNull]
        public ServiceMethodResponse Response { get; set; } = new ServiceMethodResponse();

        public bool HasBody => Parameters.Any(p => p.PassOver == ParameterPassOver.Body);
    }
}
