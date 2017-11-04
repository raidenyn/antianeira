using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Net.Http;
using System.Reflection;
using DotLiquid;

namespace Antianeira.Schema
{
    public class ServiceClientMethod: Drop
    {
        public ServiceClientMethod([NotNull] string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        [NotNull]
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;

        [NotNull]
        public MethodUrl.MethodUrl Url { get; set; } = new MethodUrl.MethodUrl();

        [CanBeNull]
        public List<ServiceClientMethodParameter> Parameters { get; set; }

        [CanBeNull]
        public MethodInfo SourceMethod { get; set; }
    }
}
